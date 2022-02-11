using DBLayer.Core.Interface;
using DBLayer.Core.Linq;
using System.Linq.Expressions;

namespace DBLayer.Persistence.Linq;

public class SqlQueryProvider : IQueryProvider, IAsyncQueryProvider
{
    private readonly IRepository _repository;
    public SqlQueryProvider(IRepository repository)
    {
        _repository = repository;
    }

    //private static readonly MethodInfo _genericCreateQueryMethod
    //    = typeof(SqlQueryProvider).GetRuntimeMethods()
    //        .Single(m => (m.Name == "CreateQuery") && m.IsGenericMethod);

    //private readonly MethodInfo _genericExecuteMethod;

    //private readonly IQueryCompiler _queryCompiler;
    //public SqlQueryProvider([NotNull] IQueryCompiler queryCompiler)
    //{
    //}

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
        var query = new SqlQueryable<TElement>(this, expression);
        return query;
    }

    public IQueryable CreateQuery(Expression expression)
    {
        var query = new SqlQueryable<object>(this, expression);
        return query;
    }

    public T Execute<T>(Expression expression)
    {
        //解析表达式，这里的T可能是泛型本身，也可能是集合，或者是动态值
        MethodCallExpression methodCall = expression as MethodCallExpression;
        Expression<Func<T, bool>> result = null;
        while (methodCall != null)
        {
            Expression method = methodCall.Arguments[0];
            Expression lambda = methodCall.Arguments[1];
            LambdaExpression right = (lambda as UnaryExpression).Operand as LambdaExpression;
            if (result == null)
            {
                result = Expression.Lambda<Func<T, bool>>(right.Body, right.Parameters);
            }
            else
            {
                Expression left = result.Body;
                Expression temp = Expression.And(right.Body, left);
                result = Expression.Lambda<Func<T, bool>>(temp, result.Parameters);
            }
            methodCall = method as MethodCallExpression;
        }

        var cmdText = string.Format("SELECT * FROM {0}", typeof(T).Name);
        if (result != null)
        {
            var resolve = new ResolveExpression(_repository.DataSource);
            resolve.ResolveToSql(result);
            cmdText = string.Format("{0} WHERE {1}", cmdText, resolve.SqlWhere);
            //Command.Parameters.AddRange(resolve.Paras);
            //_repository.GetEntity<T>(cmdText,resolve.Paras);
        }


        //_resolveExpression.ResolveToSql(result);
        //var source = new DBSql().FindAs<T>(result);
        //var t = _repository.GetEntity<T>(result);
        return default;
    }

    public object Execute(Expression expression)
    {
        var result = Execute<object>(expression);
        return result;
    }

    public async Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
    {
        //throw new NotImplementedException();
        return default;
    }

    //public async IEnumerable<T> FindAs<T>(Expression<Func<T, bool>> lambdawhere,
    //    params string[] inclusionList)
    //{
    //    //var (cmdText, paramerArray) = _repository.GetEntityText(where, (whereStr, paramerList) => {
    //    //    var orderStr = _dataSource.Order(order);
    //    //    var text = _pagerGenerator.GetSelectCmdText<T>(_dataSource, ref paramerList, whereStr, orderStr, top, inclusionList);
    //    //    return text.ToString();
    //    //});

    //    //var result = await GetEntityListAsync<T>(cmdText.ToString(), CommandType.Text, paramerArray, inclusionList);


    //    var cmdText = string.Format("SELECT * FROM {0}", typeof(T).Name);
    //    if (lambdawhere != null)
    //    {
    //        ResolveExpression resolve = new ResolveExpression(_repository.DataSource);
    //        resolve.ResolveToSql(lambdawhere);
    //        cmdText = string.Format("{0} WHERE {1}", cmdText, resolve.SqlWhere);
    //        Command.Parameters.AddRange(resolve.Paras);
    //    }
    //    var result = _repository.GetEntityListAsync<T>(inclusionList);
    //}
}
