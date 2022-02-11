using System.Linq.Expressions;

namespace DBLayer.Core.Linq;

public interface IAsyncQueryProvider : IQueryProvider
{
    /// <summary>
    ///     Executes the strongly-typed query represented by a specified expression tree asynchronously.
    /// </summary>
    Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default);
}
