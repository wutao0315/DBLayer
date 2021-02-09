using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;

namespace DBLayer.Persistence.Linq
{
    public class SqlQueryProvider : IQueryProvider
    {
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
            throw new NotImplementedException();
        }

        public T Execute<T>(Expression expression)
        {
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

            //var source = new DBSql().FindAs<T>(result);
            //dynamic _temp = source;
            //T t = (T)_temp;
            //return t;
            return default;
        }

        public object Execute(Expression expression)
        {
            throw new NotImplementedException();
        }
    }
}
