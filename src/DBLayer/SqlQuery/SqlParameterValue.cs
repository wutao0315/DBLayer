using System.Diagnostics;

namespace DBLayer.SqlQuery;

using DBLayer.Common;

[DebuggerDisplay("{ProviderValue}, {DbDataType}")]
public class SqlParameterValue
{
	public SqlParameterValue(object? providerValue, DbDataType dbDataType)
	{
		ProviderValue = providerValue;
		DbDataType    = dbDataType;
	}

	public object?    ProviderValue { get; }
	public DbDataType DbDataType    { get; }
}
