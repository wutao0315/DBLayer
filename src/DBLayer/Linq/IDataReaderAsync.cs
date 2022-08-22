using System.Data.Common;

namespace DBLayer.Linq;

public interface IDataReaderAsync : IDisposable,IAsyncDisposable
{
	DbDataReader DataReader { get; }
	Task<bool>   ReadAsync(CancellationToken cancellationToken);
}
