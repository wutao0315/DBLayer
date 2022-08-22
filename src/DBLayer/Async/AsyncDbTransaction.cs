using System.Data.Common;

namespace DBLayer.Async;

/// <summary>
/// Basic <see cref="IAsyncDbTransaction"/> implementation with fallback to synchronous operations if corresponding functionality
/// missing from <see cref="DbTransaction"/>.
/// </summary>
public class AsyncDbTransaction : IAsyncDbTransaction
{
	internal protected AsyncDbTransaction(DbTransaction transaction)
	{
		Transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
	}

	public DbTransaction Transaction { get; }

	public virtual void Commit  () => Transaction.Commit();
	public virtual void Rollback() => Transaction.Rollback();

	public virtual async Task CommitAsync(CancellationToken cancellationToken)
	{
		await Transaction.CommitAsync(cancellationToken);
	}

	public virtual async Task RollbackAsync(CancellationToken cancellationToken)
	{
		await Transaction.RollbackAsync(cancellationToken);
	}

	#region IDisposable
	public virtual void Dispose() => Transaction.Dispose();
	#endregion

	#region IAsyncDisposable
	public virtual async ValueTask DisposeAsync()
	{
		if (Transaction is IAsyncDisposable asyncDisposable)
			await asyncDisposable.DisposeAsync();

		Dispose();
	}
	#endregion
}
