using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DBLayer.Core.Linq
{
    public interface IAsyncQueryProvider : IQueryProvider
    {
        /// <summary>
        ///     Executes the strongly-typed query represented by a specified expression tree asynchronously.
        /// </summary>
        Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default);
    }
}
