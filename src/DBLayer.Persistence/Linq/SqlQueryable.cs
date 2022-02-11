using DBLayer.Core.Interface;
using System.Collections;
using System.Linq.Expressions;

namespace DBLayer.Persistence.Linq;

public class SqlQueryable<T> : IQueryable<T>, IOrderedQueryable<T>
{
    protected readonly Expression _expression;
    protected readonly IQueryProvider _provider;

    public SqlQueryable(IRepository repository)
    {
        _provider = new SqlQueryProvider(repository);
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

    public virtual Type ElementType => typeof(SqlQueryable<>);

    public Expression Expression => _expression;

    public IQueryProvider Provider => _provider;
}
