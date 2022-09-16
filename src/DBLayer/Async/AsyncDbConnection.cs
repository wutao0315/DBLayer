using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;


namespace DBLayer.Async;

/// <summary>
/// Implements <see cref="IAsyncDbConnection"/> wrapper over <see cref="DbConnection"/> instance with
/// synchronous implementation of asynchronous methods.
/// Providers with async operations support could override its methods with asynchronous implementations.
/// </summary>
public class AsyncDbConnection : IAsyncDbConnection
{
    protected internal AsyncDbConnection(DbConnection connection)
    {
        Connection = connection ?? throw new ArgumentNullException(nameof(connection));
    }

    public virtual DbConnection Connection { get; }

    public virtual DbConnection? TryClone()
    {
        try
        {
            return Connection is ICloneable cloneable
                ? (DbConnection)cloneable.Clone()
                : null;
        }
        catch
        {
            // this try-catch added to handle errors like this one from MiniProfiler's ProfiledDbConnection
            // "NotSupportedException : Underlying SqliteConnection is not cloneable"
            // because wrapper implements ICloneable but wrapped connection doesn't
            // exception-less solution will be always return null for wrapped connections which is also meh
            return null;
        }
    }

    [AllowNull]
    public virtual string ConnectionString
    {
        get => Connection.ConnectionString;
        set => Connection.ConnectionString = value;
    }

    public virtual ConnectionState State => Connection.State;

    public virtual DbCommand CreateCommand() => Connection.CreateCommand();

    public virtual void Open() => Connection.Open();
    public virtual Task OpenAsync(CancellationToken cancellationToken) => Connection.OpenAsync(cancellationToken);

    public virtual void Close() => Connection.Close();
    public virtual async Task CloseAsync()
    {
        await Connection.CloseAsync();
    }

    public virtual IAsyncDbTransaction BeginTransaction() => AsyncFactory.Create(Connection.BeginTransaction());
    public virtual IAsyncDbTransaction BeginTransaction(IsolationLevel isolationLevel) => AsyncFactory.Create(Connection.BeginTransaction(isolationLevel));

    public virtual async ValueTask<IAsyncDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        var transaction = await Connection.BeginTransactionAsync(cancellationToken)
            .ConfigureAwait(Common.Configuration.ContinueOnCapturedContext);
        return AsyncFactory.Create(transaction);
    }

    public virtual async ValueTask<IAsyncDbTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken)
    {
        var transaction = await Connection.BeginTransactionAsync(isolationLevel, cancellationToken)
            .ConfigureAwait(Common.Configuration.ContinueOnCapturedContext);
        return AsyncFactory.Create(transaction);
    }

    public virtual void Dispose() => Connection.Dispose();

    public virtual async ValueTask DisposeAsync()
    {
        if (Connection is IAsyncDisposable asyncDisposable)
            await asyncDisposable.DisposeAsync();

        Dispose();
    }
}
