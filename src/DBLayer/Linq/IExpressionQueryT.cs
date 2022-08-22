using System.Linq.Expressions;
using DBLayer.Async;

namespace DBLayer.Linq;

public interface IExpressionQuery<out T> : IOrderedQueryable<T>, IQueryProviderAsync, IExpressionQuery
{
	new Expression Expression { get; set; }
}
