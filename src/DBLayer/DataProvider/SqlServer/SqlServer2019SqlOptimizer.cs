using DBLayer.SqlProvider;

namespace DBLayer.DataProvider.SqlServer;
class SqlServer2019SqlOptimizer : SqlServer2012SqlOptimizer
{
	public SqlServer2019SqlOptimizer(SqlProviderFlags sqlProviderFlags) : base(sqlProviderFlags, SqlServerVersion.v2019)
	{
	}
}
