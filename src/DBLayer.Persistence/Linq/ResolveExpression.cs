using DBLayer;
using DBLayer.Interface;
using System.Data.Common;
using System.Linq.Expressions;
using FuncExpression = DBLayer.ConditionExtensions;

namespace DBLayer.Persistence.Linq;

public class ResolveExpression
{
    private readonly IDataSource _dataSource;
    public ResolveExpression(IDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public Dictionary<string, object> Argument;
    public string SqlWhere;
    public DbParameter[] Paras;
    private int index = 0;

    /// <summary>
    /// 解析lamdba，生成Sql查询条件
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public void ResolveToSql(Expression expression)
    {
        this.index = 0;
        this.Argument = new Dictionary<string, object>();
        this.SqlWhere = Resolve(expression);
        this.Paras = Argument.Select(x => _dataSource.CreateParameter(x.Key, x.Value)).ToArray();
    }

    private object? GetValue(Expression expression)
    {
        if (expression is ConstantExpression cst)
            return cst.Value;
        if (expression is UnaryExpression unary)
        {
            LambdaExpression lambda = Expression.Lambda(unary.Operand);
            Delegate fn = lambda.Compile();
            return fn.DynamicInvoke(null);
        }
        if (expression is MemberExpression member)
        {
            string name = member.Member.Name;
            var constant = member.Expression as ConstantExpression;
            if (constant == null)
                throw new Exception("取值时发生异常" + member);
            return constant.Value.GetType().GetFields().First(x => x.Name == name).GetValue(constant.Value);
        }
        throw new Exception("无法获取值" + expression);
    }

    private string Resolve(Expression expression)
    {
        if (expression is LambdaExpression lambda)
        {
            expression = lambda.Body;
            return Resolve(expression);
        }
        if (expression is BinaryExpression binary)//解析二元运算符
        {
            if (binary.Left is MemberExpression)
            {
                object value = GetValue(binary.Right);
                return ResolveFunc(binary.Left, value, binary.NodeType);
            }
            if (binary.Left is MethodCallExpression && (binary.Right is UnaryExpression || binary.Right is MemberExpression))
            {
                object value = GetValue(binary.Right);
                return ResolveLinqToObject(binary.Left, value, binary.NodeType);
            }
        }
        if (expression is UnaryExpression unary)//解析一元运算符
        {
            if (unary.Operand is MethodCallExpression)
            {
                return ResolveLinqToObject(unary.Operand, false);
            }
            if (unary.Operand is MemberExpression)
            {
                return ResolveFunc(unary.Operand, false, ExpressionType.Equal);
            }
        }
        if (expression is MethodCallExpression)//解析扩展方法
        {
            return ResolveLinqToObject(expression, true);
        }
        if (expression is MemberExpression)//解析属性。。如x.Deletion
        {
            return ResolveFunc(expression, true, ExpressionType.Equal);
        }
        var body = expression as BinaryExpression;
        if (body == null)
            throw new Exception("无法解析" + expression);
        var operate = GetOperator(body.NodeType);
        var left = Resolve(body.Left);
        var right = Resolve(body.Right);
        var result = $"({left} {operate } {right})";
        return result;
    }

    /// <summary>
    /// 根据条件生成对应的sql查询操作符
    /// </summary>
    /// <param name="expressiontype"></param>
    /// <returns></returns>
    private string GetOperator(ExpressionType expressiontype)
    {
        switch (expressiontype)
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
                throw new Exception($"不支持{expressiontype}此种运算符查找！");
        }
    }

    private string ResolveFunc(Expression left, object val, ExpressionType expressiontype)
    {
        var name = (left as MemberExpression).Member.GetFieldName();
        var operate = GetOperator(expressiontype);
        var value = val.ToString();
        var compName = SetArgument(name, value);
        var dbName = string.Format(_dataSource.DbFactory.DbProvider.FieldFormat, name);
        var result = $"({dbName} {operate} {compName})";
        return result;
    }

    private string ResolveLinqToObject(Expression expression, object value, ExpressionType? expressiontype = null)
    {
        var methodCall = expression as MethodCallExpression;
        var methodName = methodCall.Method.Name;
        switch (methodName)//这里其实还可以改成反射调用，不用写switch
        {
            case nameof(FuncExpression.Like):
                return LikeCall(methodCall, true);
            case nameof(FuncExpression.NotLike):
                return LikeCall(methodCall, false);
            case nameof(FuncExpression.In):
                return In(methodCall, true);
            case nameof(FuncExpression.NotIn):
                return In(methodCall, false);
            case nameof(FuncExpression.Less):
                return Operate(methodCall, ExpressionType.LessThan);
            case nameof(FuncExpression.LessEqual):
                return Operate(methodCall, ExpressionType.LessThanOrEqual);
            case nameof(FuncExpression.Greater):
                return Operate(methodCall, ExpressionType.GreaterThan);
            case nameof(FuncExpression.GreaterEqual):
                return Operate(methodCall, ExpressionType.GreaterThanOrEqual);
            //Enumerable
            case nameof(Enumerable.Contains):
                if (methodCall.Object != null)
                    return Like(methodCall, value);
                return In(methodCall, value);
            case nameof(Enumerable.Count):
                return Len(methodCall, value, expressiontype.Value);
            case nameof(Enumerable.LongCount):
                return Len(methodCall, value, expressiontype.Value);
            default:
                throw new Exception($"不支持{methodName}方法的查找！");
        }
    }

    private string SetArgument(string name, string value)
    {
        name = _dataSource.DbFactory.DbProvider.ParameterPrefix + name;
        string temp = name;
        while (Argument.ContainsKey(temp))
        {
            temp = $"{name}_{index}";
            index += 1;
        }
        Argument[temp] = value;
        return temp;
    }

    private string In(MethodCallExpression expression, object isTrue)
    {
        var argument1 = expression.Arguments[0];
        var argument2 = expression.Arguments[1] as MemberExpression;
        var fieldValue = GetValue(argument1);
        object[] array = fieldValue as object[];
        var setInPara = new List<string>();
        for (int i = 0; i < array.Length; i++)
        {
            var name_para = "ist_param_" + i;
            var value = array[i].ToString();
            var key = SetArgument(name_para, value);
            setInPara.Add(key);
        }
        var name = argument2.Member.GetFieldName();
        var operate = Convert.ToBoolean(isTrue) ? "IN" : "NOT IN";
        var compName = string.Join(",", setInPara);
        var dbName = string.Format(_dataSource.DbFactory.DbProvider.FieldFormat, name);
        var result = $"{dbName} {operate}({compName})";
        return result;
    }

    private string Operate(MethodCallExpression expression, ExpressionType expressionType)
    {
        Expression argument = expression.Arguments[0];
        object tempVal = GetValue(argument);
        var value = tempVal.ToString();
        var name = (expression.Object as MemberExpression).Member.GetFieldName();
        var compName = SetArgument(name, value);
        var operate = GetOperator(expressionType);
        var dbName = string.Format(_dataSource.DbFactory.DbProvider.FieldFormat, name);
        var result = $"{dbName} {operate} {compName}";
        return result;
    }

    private string Like(MethodCallExpression expression, object isTrue)
    {
        Expression argument = expression.Arguments[0];
        object tempVal = GetValue(argument);
        var value = $"%{tempVal}%";
        var name = (expression.Object as MemberExpression).Member.GetFieldName();
        var compName = SetArgument(name, value);
        var operate = Convert.ToBoolean(isTrue) ? "LIKE" : "NOT LIKE";
        var dbName = string.Format(_dataSource.DbFactory.DbProvider.FieldFormat, name);
        var result = $"{dbName} {operate} {compName}";
        return result;
    }
    private string LikeCall(MethodCallExpression expression, object isTrue)
    {
        Expression argument = expression.Arguments[0];
        object tempVal = GetValue(argument);
        var value = tempVal.ToString();
        var name = (expression.Object as MemberExpression).Member.GetFieldName();
        var compName = SetArgument(name, value);
        var operate = Convert.ToBoolean(isTrue) ? "LIKE" : "NOT LIKE";
        var dbName = string.Format(_dataSource.DbFactory.DbProvider.FieldFormat, name);
        var result = $"{dbName} {operate} {compName}";
        return result;
    }

    private string Len(MethodCallExpression expression, object value, ExpressionType expressiontype)
    {
        object name = (expression.Arguments[0] as MemberExpression).Member.GetFieldName();
        var operate = GetOperator(expressiontype);
        var compName = SetArgument(name.ToString(), value.ToString());
        var dbName = string.Format(_dataSource.DbFactory.DbProvider.FieldFormat, name);
        var result = $"LEN({dbName}){operate}{compName}";
        return result;
    }
}
