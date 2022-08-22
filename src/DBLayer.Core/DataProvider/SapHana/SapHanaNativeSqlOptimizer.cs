using DBLayer.SqlProvider;

namespace DBLayer.DataProvider.SapHana;

class SapHanaNativeSqlOptimizer : SapHanaSqlOptimizer
{
	public SapHanaNativeSqlOptimizer(SqlProviderFlags sqlProviderFlags) : base(sqlProviderFlags)
	{
	}
}

