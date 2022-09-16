using DBLayer;
using DBLayer.Interface;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace DBLayer.Persistence;

internal static class FuncExtensions
{
    #region Where Expression
    internal static StringBuilder Where<T>(this IDataSource dataSource, Expression<Func<T, bool>>? func, ref List<DbParameter> paramerList) where T : new()
    {
        var result = new StringBuilder();
        if (func != null)
        {
            var index = 0;
            if (func.Body is BinaryExpression be)
            {
                result = BinarExpressionProvider(dataSource, be.Left, be.Right, be.NodeType, ref index, ref paramerList);
                //去除多余括号
                result.Remove(0, 1);
                result.Length -= 1;
                result.Insert(0, " WHERE ");
            }
            else if (func.Body is MethodCallExpression)
            {
                result = ExpressionRouter(dataSource, func.Body, DirAway.Left, ref index, ref paramerList);
                result.Insert(0, " WHERE ");
            }
        }
        return result;
    }

    static StringBuilder BinarExpressionProvider(this IDataSource dataSource, Expression left, Expression right, ExpressionType type, ref int index, ref List<DbParameter> paramerList)
    {
        var sb = new StringBuilder("(");
        //先处理左边
        sb.Append(ExpressionRouter(dataSource, left, DirAway.Left, ref index, ref paramerList));
        sb.Append(ExpressionTypeCast(type));

        //再处理右边
        var tmpStr = ExpressionRouter(dataSource, right, DirAway.Right, ref index, ref paramerList);
        if (tmpStr.ToString() == "null")
        {
            if (sb.ToString().EndsWith(" ="))
            {
                sb.Length -= 2;
                sb.Append(" IS NULL");
            }
            else if (sb.ToString().EndsWith("<>"))
            {
                sb.Length -= 2;
                sb.Append(" IS NOT NULL");
            }
        }
        else
        {
            sb.Append(tmpStr);
        }
        sb.Append(")");
        return sb;
    }

    static StringBuilder ExpressionRouter(this IDataSource dataSource, Expression exp, DirAway away, ref int index, ref List<DbParameter> paramerList, bool isFunc = false)
    {
        var sb = new StringBuilder();

        if (exp is BinaryExpression be)
        {
            sb = BinarExpressionProvider(dataSource, be.Left, be.Right, be.NodeType, ref index, ref paramerList);
        }
        else if (exp is MemberExpression me)
        {
            if (away == DirAway.Left)
            {
                sb.AppendFormat(dataSource.DbFactory.DbProvider.FieldFormat, me.Member.GetFieldName());
            }
            else
            {
                //把member的值读出来
                sb = ExpressionRouter(dataSource, GetReduceOrConstant(me), away, ref index, ref paramerList, isFunc);
            }

        }
        else if (exp is NewArrayExpression ae)
        {
            if (isFunc)
            {
                var tmpstr = new StringBuilder();
                foreach (var ex in ae.Expressions)
                {
                    tmpstr.Append(ExpressionRouter(dataSource, ex, DirAway.Left, ref index, ref paramerList));
                    tmpstr.Append(',');
                }

                if (tmpstr.Length > 0)
                {
                    tmpstr.Length -= 1;
                    var result = dataSource.DbFactory.DbProvider.ParameterPrefix + "fs_param_" + index;

                    sb.Append(result);
                    paramerList.Add(dataSource.CreateParameter(result, tmpstr.ToString()));
                    index++;
                }
            }
            else
            {
                var tmpstr = new StringBuilder();
                var arrayIndex = 0;
                foreach (var ex in ae.Expressions)
                {
                    var result = string.Concat(dataSource.DbFactory.DbProvider.ParameterPrefix, "arr_param_", index, "_" + arrayIndex);
                    var value = ExpressionRouter(dataSource, ex, DirAway.Left, ref index, ref paramerList);

                    paramerList.Add(dataSource.CreateParameter(result, value.ToString()));

                    tmpstr.Append(result);
                    tmpstr.Append(',');

                    arrayIndex++;
                }

                if (tmpstr.Length > 0)
                {
                    tmpstr.Length -= 1;
                    sb.Append(tmpstr);
                    index++;
                }
            }
        }
        else if (exp is MethodCallExpression mce)
        {
            if (mce.Method.Name == nameof(ConditionExtensions.Like))
            {
                sb.AppendFormat("({0} LIKE {1})", ExpressionRouter(dataSource, mce.Arguments[0], DirAway.Left, ref index, ref paramerList), ExpressionRouter(dataSource, mce.Arguments[1], DirAway.Right, ref index, ref paramerList));
            }
            else if (mce.Method.Name == nameof(ConditionExtensions.NotLike))
            {
                sb.AppendFormat("({0} NOT LIKE {1})", ExpressionRouter(dataSource, mce.Arguments[0], DirAway.Left, ref index, ref paramerList), ExpressionRouter(dataSource, mce.Arguments[1], DirAway.Right, ref index, ref paramerList));
            }
            else if (mce.Method.Name == nameof(ConditionExtensions.Less))
            {
                sb.AppendFormat("({0} < {1})", ExpressionRouter(dataSource, mce.Arguments[0], DirAway.Left, ref index, ref paramerList), ExpressionRouter(dataSource, mce.Arguments[1], DirAway.Right, ref index, ref paramerList));
            }
            else if (mce.Method.Name == nameof(ConditionExtensions.LessEqual))
            {
                sb.AppendFormat("({0} <= {1})", ExpressionRouter(dataSource, mce.Arguments[0], DirAway.Left, ref index, ref paramerList), ExpressionRouter(dataSource, mce.Arguments[1], DirAway.Right, ref index, ref paramerList));
            }
            else if (mce.Method.Name == nameof(ConditionExtensions.Greater))
            {
                sb.AppendFormat("({0} > {1})", ExpressionRouter(dataSource, mce.Arguments[0], DirAway.Left, ref index, ref paramerList), ExpressionRouter(dataSource, mce.Arguments[1], DirAway.Right, ref index, ref paramerList));
            }
            else if (mce.Method.Name == nameof(ConditionExtensions.GreaterEqual))
            {
                sb.AppendFormat("({0} >= {1})", ExpressionRouter(dataSource, mce.Arguments[0], DirAway.Left, ref index, ref paramerList), ExpressionRouter(dataSource, mce.Arguments[1], DirAway.Right, ref index, ref paramerList));
            }
            else if (mce.Method.Name == nameof(ConditionExtensions.In))
            {
                sb.AppendFormat("{0} IN ({1})", ExpressionRouter(dataSource, mce.Arguments[0], DirAway.Left, ref index, ref paramerList), ExpressionRouter(dataSource, mce.Arguments[1], DirAway.Right, ref index, ref paramerList));
            }
            else if (mce.Method.Name == nameof(ConditionExtensions.NotIn))
            {
                sb.AppendFormat("{0} NOT IN ({1})", ExpressionRouter(dataSource, mce.Arguments[0], DirAway.Left, ref index, ref paramerList), ExpressionRouter(dataSource, mce.Arguments[1], DirAway.Right, ref index, ref paramerList));
            }
            //else if (mce.Method.Name == "Contains")
            //{
            //    if (mce.Object != null)
            //    {
            //        return Like(MethodCall);
            //    }
            //    return In(mce, value);
            //}
            else if (mce.Method.Name == nameof(ConditionExtensions.InFunc))
            {
                var leftString = ExpressionRouter(dataSource, mce.Arguments[0], DirAway.Left, ref index, ref paramerList);
                var rightString = ExpressionRouter(dataSource, mce.Arguments[1], DirAway.Right, ref index, ref paramerList, true);

                var infunc = dataSource.PagerGenerator.GetInFunc(() =>
                {
                    return leftString;
                }, () =>
                {
                    return rightString;
                });

                sb.Append(infunc);
            }
            else if (mce.Method.Name == nameof(ConditionExtensions.NotInFunc))
            {
                var leftString = ExpressionRouter(dataSource, mce.Arguments[0], DirAway.Left, ref index, ref paramerList);
                var rightString = ExpressionRouter(dataSource, mce.Arguments[1], DirAway.Right, ref index, ref paramerList, true);

                var infunc = dataSource.PagerGenerator.GetNotInFunc(() =>
                {
                    return leftString;
                }, () =>
                {
                    return rightString;
                });

                sb.Append(infunc);
            }
            else if (away == DirAway.Right)
            {
                sb.Append(ExpressionRouter(dataSource, GetReduceOrConstant(mce), away, ref index, ref paramerList, isFunc));
            }

        }
        else if (exp is ConstantExpression ce)
        {
            if (away == DirAway.Right)
            {
                if (ce.Value is Array arrayData)
                {
                    var list = new List<Expression>();
                    foreach (var item in arrayData)
                    {
                        list.Add(Expression.Constant(item));
                    }

                    sb.Append(ExpressionRouter(dataSource, Expression.NewArrayInit(arrayData.GetValue(0)!.GetType(), list), away, ref index, ref paramerList, isFunc));
                }
                else
                {
                    var result = dataSource.DbFactory.DbProvider.ParameterPrefix + "wh_param_" + index;
                    sb.Append(result);
                    if (ce.Value == null || ce.Value is DBNull)
                    {
                        paramerList.Add(dataSource.CreateParameter(result, DBNull.Value));

                    }
                    else if (ce.Value is ValueType)
                    {
                        paramerList.Add(dataSource.CreateParameter(result, ce.Value));
                    }
                    else if (ce.Value is string || ce.Value is DateTime || ce.Value is char)
                    {
                        paramerList.Add(dataSource.CreateParameter(result, ce.Value));
                    }
                    index++;
                }
            }
            else
            {
                if (ce.Value == null)
                {
                    sb.Append("null");
                }
                else
                {
                    sb.Append(ce.Value);
                }
            }
        }
        else if (exp is UnaryExpression ue)
        {
            return ExpressionRouter(dataSource, ue.Operand, away, ref index, ref paramerList, isFunc);
        }
        else
        {
            sb.Append(ExpressionRouter(dataSource, GetReduceOrConstant(exp), away, ref index, ref paramerList, isFunc));
        }

        return sb;
    }

    static string ExpressionTypeCast(ExpressionType type)
    {
        switch (type)
        {
            case ExpressionType.And:
            case ExpressionType.AndAlso:
                return " AND ";
            case ExpressionType.Equal:
                return " =";
            case ExpressionType.GreaterThan:
                return " >";
            case ExpressionType.GreaterThanOrEqual:
                return ">=";
            case ExpressionType.LessThan:
                return "<";
            case ExpressionType.LessThanOrEqual:
                return "<=";
            case ExpressionType.NotEqual:
                return "<>";
            case ExpressionType.Or:
            case ExpressionType.OrElse:
                return " OR ";
            case ExpressionType.Add:
            case ExpressionType.AddChecked:
                return "+";
            case ExpressionType.AddAssign:
            case ExpressionType.AddAssignChecked:
                return "+=";
            case ExpressionType.Subtract:
            case ExpressionType.SubtractChecked:
                return "-";
            case ExpressionType.SubtractAssign:
            case ExpressionType.SubtractAssignChecked:
                return "-=";
            case ExpressionType.Divide:
                return "/";
            case ExpressionType.Multiply:
            case ExpressionType.MultiplyChecked:
                return "*";
            default:
                throw new Exception($"不支持{type}此种运算符查找！");
        }
    }
    #endregion

    #region Order Expression
    internal static StringBuilder Order<T>(this IDataSource dataSource, Expression<Func<OrderExpression<T>, object>>? func) where T : new()
    {
        var result = new StringBuilder();
        if (func != null)
        {
            result.Append(" ORDER BY ");
            result.Append(dataSource.ExpressionOrderRoute<T>(func));
        }
        return result;
    }

    static StringBuilder ExpressionOrderRoute<T>(this IDataSource dataSource, Expression exp) where T : new()
    {
        var result = new StringBuilder();

        if (exp is LambdaExpression lmd)
        {
            result.Append(dataSource.ExpressionOrderRoute<T>(lmd.Body));
        }
        else if (exp is MethodCallExpression mce)
        {
            if (mce.Object is MethodCallExpression)
            {
                result.Append(dataSource.ExpressionOrderRoute<T>(mce.Object));
                result.Append(',');
            }
            if (mce.Method.Name == nameof(System.Linq.Enumerable.OrderBy))
                return result.AppendFormat("{0} ASC", dataSource.ExpressionOrderRoute<T>(mce.Arguments[0]));
            else if (mce.Method.Name == nameof(OrderExpression<T>.OrderByDesc)
                || mce.Method.Name == nameof(System.Linq.Enumerable.OrderByDescending))
                return result.AppendFormat("{0} DESC", dataSource.ExpressionOrderRoute<T>(mce.Arguments[0]));
        }
        else if (exp is MemberExpression me)
        {
            result.AppendFormat(dataSource.DbFactory.DbProvider.FieldFormat, me.Member.GetFieldName());
        }
        else if (exp is UnaryExpression ue)
        {
            result.Append(dataSource.ExpressionOrderRoute<T>(ue.Operand));
        }
        return result;
    }


    #endregion

    #region Update
    /// <summary>
    /// 更新语句生成
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="func"></param>
    /// <param name="paramerList"></param>
    /// <returns></returns>
    internal static StringBuilder Update<T>(this IDataSource dataSource, Expression<Func<T>> func, ref List<DbParameter> paramerList) where T : new()
    {
        var index = 0;
        var result = ExpressionUpdateRouter<T>(dataSource, func.Body, DirAway.Left, ref index, ref paramerList);
        return result;
    }

    /// <summary>
    /// 更新语句生成
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="exp"></param>
    /// <param name="index"></param>
    /// <param name="paramerList"></param>
    /// <returns></returns>
    static StringBuilder ExpressionUpdateRouter<T>(this IDataSource dataSource, Expression exp, DirAway away, ref int index, ref List<DbParameter> paramerList)
    {

        var result = new StringBuilder();

        if (exp is MemberInitExpression minit)
        {
            //var sb = new StringBuilder();
            foreach (var item in minit.Bindings)
            {

                if (item is MemberAssignment myItem)
                {
                    var isKey = false;
                    var isAuto = false;
                    var keyType = KeyType.SEQ;

                    var fieldName = myItem.Member.Name;
                    var df = myItem.Member.GetCustomAttribute<DataFieldAttribute>(true);
                    if (df != null)
                    {
                        fieldName = df.FieldName;
                        isKey = df.IsKey;
                        isAuto = df.IsAuto;
                        keyType = df.KeyType;
                    }

                    if (isKey && isAuto && keyType == KeyType.SEQ)
                    {
                        continue;
                    }
                    else
                    {
                        //result.AppendFormat(dataSource.DbFactory.DbProvider.FieldFormat, myItem.Member.GetFieldName());
                        //result.Append(" = ");
                        //result.Append(ExpressionUpdateRouter<T>(dataSource,myItem.Expression, DirAway.Right, ref index, ref paramerList));
                        //result.Append(',');

                        var fieldNameKey = string.Format(dataSource.DbFactory.DbProvider.FieldFormat, myItem.Member.GetFieldName());
                        result.Append(fieldNameKey);

                        //识别自方法体,做自增，自减，自乘，自除, 自取模
                        var dynamicExpression = myItem.Expression as MethodCallExpression;
                        if (dynamicExpression != null)
                        {
                            switch (dynamicExpression.Method.Name)
                            {
                                case nameof(ConditionExtensions.AddEqual):
                                    result.Append(" = ");
                                    result.Append(fieldNameKey);
                                    result.Append(" + ");
                                    break;
                                case nameof(ConditionExtensions.SubEqual):
                                    result.Append(" = ");
                                    result.Append(fieldNameKey);
                                    result.Append(" - ");
                                    break;
                                case nameof(ConditionExtensions.MultiEqual):
                                    result.Append(" = ");
                                    result.Append(fieldNameKey);
                                    result.Append(" * ");
                                    break;
                                case nameof(ConditionExtensions.DivEqual):
                                    result.Append(" = ");
                                    result.Append(fieldNameKey);
                                    result.Append(" / ");
                                    break;
                                case nameof(ConditionExtensions.ModEqual):
                                    result.Append(" = ");
                                    result.Append(fieldNameKey);
                                    result.Append(" % ");
                                    break;
                                default:
                                    result.Append(" = ");
                                    break;
                            }
                        }
                        else
                        {
                            result.Append(" = ");
                        }

                        result.Append(ExpressionUpdateRouter<T>(dataSource, myItem.Expression, DirAway.Right, ref index, ref paramerList));
                        result.Append(',');
                    }
                }
            }
            if (result.Length > 0)
            {
                result.Length -= 1;
            }
        }
        else if (exp is MemberExpression mbe)
        {
            if (mbe.Member.MemberType == MemberTypes.Property)
            {
                if (away == DirAway.Left)
                {
                    result.AppendFormat(dataSource.DbFactory.DbProvider.FieldFormat, mbe.Member.GetFieldName());
                }
                else
                {
                    result = ExpressionUpdateRouter<T>(dataSource, GetReduceOrConstant(mbe), away, ref index, ref paramerList);
                }

            }
            else if (mbe.Member.MemberType == MemberTypes.Field)
            {
                if (away == DirAway.Left)
                {
                    T maValue = default!;
                    try
                    {
                        maValue = (T)GetMemberExpressionValue(mbe);
                    }
                    catch (Exception)
                    {
                    }
                    if (maValue != null)
                    {
                        var memberList = new List<MemberBinding>();
                        var mbeType = typeof(T);
                        //foreach (var member in mbeType.GetProperties())
                        foreach (var member in mbeType.GetCachedProperties())
                        {
                            if (member.Key.MemberType == MemberTypes.Property)
                            {
                                var popValue = member.Value.Getter(maValue);//.GetValue(maValue, null);
                                var binding = Expression.Bind(member.Key, Expression.Constant(popValue, member.Key.PropertyType));
                                memberList.Add(binding);
                            }
                        }

                        //mbeCst.Value
                        var myMem = Expression.MemberInit(Expression.New(mbeType), memberList.ToArray());
                        result = ExpressionUpdateRouter<T>(dataSource, myMem, away, ref index, ref paramerList);
                    }
                }
                else
                {
                    result = ExpressionUpdateRouter<T>(dataSource, GetReduceOrConstant(mbe), away, ref index, ref paramerList);
                }

            }
        }
        else if (exp is UnaryExpression uny)
        {
            result = ExpressionUpdateRouter<T>(dataSource, uny.Operand, away, ref index, ref paramerList);
        }
        else if (exp is ConstantExpression ce)
        {
            var paramName = dataSource.DbFactory.DbProvider.ParameterPrefix + "ud_param_" + index;

            if (ce.Value == null || ce.Value is DBNull)
            {
                paramerList.Add(dataSource.CreateParameter(paramName, DBNull.Value));
            }
            else if (ce.Value is ValueType)
            {
                paramerList.Add(dataSource.CreateParameter(paramName, ce.Value));
            }
            else if (ce.Value is string || ce.Value is DateTime || ce.Value is char)
            {
                paramerList.Add(dataSource.CreateParameter(paramName, ce.Value));
            }
            else
            {
                paramerList.Add(dataSource.CreateParameter(paramName, ce.Value));
            }

            result.Append(paramName);
            index++;
        }
        else
        {
            result = ExpressionUpdateRouter<T>(dataSource, GetReduceOrConstant(exp), away, ref index, ref paramerList);
        }

        return result;
    }
    #endregion

    #region Insert
    /// <summary>
    /// 更新语句生成
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="func"></param>
    /// <param name="paramerList"></param>
    /// <returns></returns>
    internal static (StringBuilder, StringBuilder) Insert<T>(this IDataSource dataSource, Expression<Func<T>> func, ref object? newID, ref List<DbParameter> paramerList, IGenerator generater, string userText) where T : new()
    {
        var index = 0;
        var isBaseEntity = typeof(T).HasImplementedRawGeneric(typeof(BaseEntity<>));
        var result = ExpressionInsertRouter<T>(dataSource, func.Body, DirAway.Left, ref index, ref newID, ref paramerList, generater, isBaseEntity, userText);
        return result;
    }
    /// <summary>
    /// 更新语句生成
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="exp"></param>
    /// <param name="index"></param>
    /// <param name="paramerList"></param>
    /// <returns></returns>
    static (StringBuilder, StringBuilder) ExpressionInsertRouter<T>(this IDataSource dataSource, Expression exp, DirAway away, ref int index, ref object newID, ref List<DbParameter> paramerList, IGenerator generater, bool isBaseEntity, string userText) where T : new()
    {
        var resultKey = new StringBuilder();
        var resultValue = new StringBuilder();
        if (exp is MemberInitExpression minit)
        {
            var sbField = new StringBuilder();
            var sbValue = new StringBuilder();
            foreach (var item in minit.Bindings)
            {
                if (item is MemberAssignment myItem)
                {

                    var isKey = false;
                    var isAuto = false;
                    var keyType = KeyType.SEQ;

                    var fieldName = myItem.Member.Name;
                    var df = myItem.Member.GetCustomAttribute<DataFieldAttribute>(true);
                    if (df != null)
                    {
                        fieldName = df.FieldName;
                        isKey = df.IsKey;
                        isAuto = df.IsAuto;
                        keyType = df.KeyType;
                    }

                    if (isKey && isAuto)
                    {
                        if (keyType == KeyType.SEQ)
                        {
                            dataSource.PagerGenerator.ProcessInsertId<T>(fieldName, ref sbField, ref sbValue);
                            continue;
                        }
                        else
                        {

                            newID = GetMemberExpressionValue(myItem.Expression);

                            if (newID != null && !string.IsNullOrEmpty(newID.ToString()))
                            {
                                try
                                {
                                    var currentLongId = -1L;
                                    var currentGuidId = Guid.Empty;
                                    if (long.TryParse(newID.ToString(), out currentLongId))
                                    {
                                        if (currentLongId <= 0)
                                        {
                                            newID = generater.Generate();
                                        }
                                    }
                                    else if (Guid.TryParse(newID.ToString(), out currentGuidId))
                                    {
                                        if (currentGuidId == Guid.Empty)
                                        {
                                            newID = generater.Generate();
                                        }
                                    }
                                }
                                catch
                                {
                                    newID = generater.Generate();
                                }
                            }
                            else
                            {
                                newID = generater.Generate();
                            }

                            sbField.AppendFormat(dataSource.DbFactory.DbProvider.FieldFormat, fieldName);
                            sbField.Append(',');
                            var (_, valueRight) = ExpressionInsertRouter<T>(dataSource, Expression.Constant(newID), DirAway.Right, ref index, ref newID, ref paramerList, generater, isBaseEntity, userText);
                            sbValue.Append(valueRight);
                            sbValue.Append(',');
                        }
                    }
                    else
                    {
                        sbField.AppendFormat(dataSource.DbFactory.DbProvider.FieldFormat, fieldName);
                        sbField.Append(',');
                        var (_, valueRight) = ExpressionInsertRouter<T>(dataSource, myItem.Expression, DirAway.Right, ref index, ref newID, ref paramerList, generater, isBaseEntity, userText);
                        sbValue.Append(valueRight);
                        sbValue.Append(',');
                    }
                }
            }
            if (sbField.Length > 0)
            {
                sbField.Length -= 1;
                sbValue.Length -= 1;
            }

            resultKey.Append(sbField);
            resultValue.Append(sbValue);
        }
        else if (exp is MemberExpression mbe)
        {
            if (mbe.Member.MemberType == MemberTypes.Property)
            {
                if (away == DirAway.Left)
                {
                    resultKey = new StringBuilder(string.Format(dataSource.DbFactory.DbProvider.FieldFormat, mbe.Member.GetFieldName()));
                }
                else
                {
                    (resultKey, resultValue) = ExpressionInsertRouter<T>(dataSource, GetReduceOrConstant(mbe), away, ref index, ref newID, ref paramerList, generater, isBaseEntity, userText);
                }
            }
            else
            {
                if (mbe.Member.MemberType == MemberTypes.Field)
                {
                    if (away == DirAway.Left)
                    {
                        T maValue = default!;
                        try
                        {
                            maValue = (T)GetMemberExpressionValue(mbe);
                            if (isBaseEntity)
                            {
                                var updater = maValue.GetValueByPropertyName(nameof(BaseEntity<long>.Updater));
                                if (updater == null || string.IsNullOrWhiteSpace(updater.ToString()))
                                {
                                    maValue.SetValueByPropertyName(nameof(BaseEntity<long>.Updater), userText);
                                }
                                var creater = maValue.GetValueByPropertyName(nameof(BaseEntity<long>.Creater));
                                if (creater == null || string.IsNullOrWhiteSpace(creater.ToString()))
                                {
                                    maValue.SetValueByPropertyName(nameof(BaseEntity<long>.Creater), userText);
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                        if (maValue != null)
                        {
                            var memberList = new List<MemberBinding>();
                            var mbeType = typeof(T);
                            //foreach (var member in mbeType.GetProperties())
                            foreach (var member in mbeType.GetCachedProperties())
                            {
                                if (member.Key.MemberType == MemberTypes.Property)
                                {
                                    var popValue = member.Value.Getter(maValue);//.GetValue(maValue, null);
                                    var binding = Expression.Bind(member.Key, Expression.Constant(popValue, member.Key.PropertyType));
                                    memberList.Add(binding);

                                }
                            }
                            //mbeCst.Value
                            var myMem = Expression.MemberInit(Expression.New(mbeType), memberList.ToArray());
                            (resultKey, resultValue) = ExpressionInsertRouter<T>(dataSource, myMem, away, ref index, ref newID, ref paramerList, generater, isBaseEntity, userText);
                        }
                    }
                    else
                    {
                        (resultKey, resultValue) = ExpressionInsertRouter<T>(dataSource, GetReduceOrConstant(mbe), away, ref index, ref newID, ref paramerList, generater, isBaseEntity, userText);
                    }
                }
            }
        }
        else if (exp is UnaryExpression uny)
        {
            if (away == DirAway.Right)
            {
                (resultKey, resultValue) = ExpressionInsertRouter<T>(dataSource, GetReduceOrConstant(exp), away, ref index, ref newID, ref paramerList, generater, isBaseEntity, userText);
            }
            else
            {
                (resultKey, resultValue) = ExpressionInsertRouter<T>(dataSource, uny.Operand, away, ref index, ref newID, ref paramerList, generater, isBaseEntity, userText);
            }
        }
        else if (exp is ConstantExpression ce)
        {
            var paramName = dataSource.DbFactory.DbProvider.ParameterPrefix + "ist_param_" + index;

            if (ce.Value == null || ce.Value is DBNull)
            {
                paramerList.Add(dataSource.CreateParameter(paramName, DBNull.Value));
            }
            else if (ce.Value is ValueType)
            {
                paramerList.Add(dataSource.CreateParameter(paramName, ce.Value));
            }
            else if (ce.Value is string || ce.Value is DateTime || ce.Value is char)
            {
                paramerList.Add(dataSource.CreateParameter(paramName, ce.Value));
            }
            else
            {
                paramerList.Add(dataSource.CreateParameter(paramName, ce.Value));
            }
            resultValue.Append(paramName);
            index++;
        }
        else
        {
            (resultKey, resultValue) = ExpressionInsertRouter<T>(dataSource, GetReduceOrConstant(exp), away, ref index, ref newID, ref paramerList, generater, isBaseEntity, userText);
        }
        return (resultKey, resultValue);
    }
    static Expression GetReduceOrConstant(Expression exp)
    {
        if (exp.CanReduce)
        {
            return exp.Reduce();
        }
        else
        {
            return Expression.Constant(GetMemberExpressionValue(exp));
        }
    }
    static object GetMemberExpressionValue(Expression member)
    {
        var objectMember = Expression.Convert(member, typeof(object));
        var getterLambda = Expression.Lambda<Func<object>>(objectMember);
        var getter = getterLambda.Compile();
        return getter();
    }
    #endregion

    /// <summary>
    /// 利用反射返回参数集合
    /// </summary>
    /// <param name="feildname"></param>
    /// <param name="obEntity"></param>
    /// <returns></returns>
    internal static DbParameter[]? ToDbParameters(this IDataSource dataSource, object? obEntity)
    {
        if (obEntity == null)
        {
            return null;
        }

        var result = new List<DbParameter>();
        var tpEntity = obEntity.GetType();
        //var pis = tpEntity.GetProperties();
        var pis = tpEntity.GetCachedProperties();

        foreach (var item in pis)
        {
            var obj = item.Value.Getter(obEntity);//.GetValue(obEntity);
            var parameter = dataSource.CreateParameter(dataSource.DbFactory.DbProvider.ParameterPrefix + item.Key.Name, obj);
            result.Add(parameter);
        }
        return result.ToArray();
    }
}
enum DirAway
{
    Left, Right
}
