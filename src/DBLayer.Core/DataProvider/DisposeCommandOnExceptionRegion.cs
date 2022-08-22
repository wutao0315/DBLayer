using DBLayer.Data;
using System.Runtime.InteropServices;

namespace DBLayer.DataProvider;

internal class DisposeCommandOnExceptionRegion : IExecutionScope
{
	private readonly DataConnection _dataConnection;

	public DisposeCommandOnExceptionRegion(DataConnection dataConnection)
	{
		_dataConnection = dataConnection;
	}

	void IDisposable.Dispose()
	{
		// https://stackoverflow.com/questions/2830073/
		if (
			// API not exposed till netcoreapp3.0
			// https://github.com/dotnet/corefx/pull/31169
			Marshal.GetExceptionPointers() != IntPtr.Zero ||
#pragma warning disable CS0618 // GetExceptionCode obsolete
			Marshal.GetExceptionCode() != 0)
#pragma warning restore CS0618
			_dataConnection.DisposeCommand();
	}

	ValueTask IAsyncDisposable.DisposeAsync()
	{
		// https://stackoverflow.com/questions/2830073/
		if (
			// API not exposed till netcoreapp3.0
			// https://github.com/dotnet/corefx/pull/31169
			Marshal.GetExceptionPointers() != IntPtr.Zero ||
#pragma warning disable CS0618 // GetExceptionCode obsolete
			Marshal.GetExceptionCode() != 0)
#pragma warning restore CS0618
			return _dataConnection.DisposeCommandAsync();
		return default;
	}
}
