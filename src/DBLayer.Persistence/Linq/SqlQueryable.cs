using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace DBLayer.Persistence.Linq
{
    public class SqlQueryable<T> : IQueryable<T>
    {
        protected Expression _expression;
        protected IQueryProvider _provider;

        public SqlQueryable()
        {
            _provider = new SqlQueryProvider();
            _expression = Expression.Constant(this);
        }

        public SqlQueryable(IQueryProvider provider, Expression expression)
        {
            _provider = provider;
            _expression = expression;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            var result = _provider.Execute<IEnumerable>(_expression).GetEnumerator();
            return result;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var result = _provider.Execute<IEnumerable<T>>(Expression).GetEnumerator();
            return result;
        }

        public virtual Type ElementType
        {
            get { return typeof(SqlQueryable<>); }
        }

        public Expression Expression
        {
            get { return _expression; }
        }

        public IQueryProvider Provider
        {
            get { return _provider; }
        }
    }
}
