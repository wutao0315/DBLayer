using System;
using DBLayer.Mapping;
using DBLayer.SqlProvider;

namespace DBLayer.DataProvider.SqlServer;
class SqlServer2019SqlBuilder : SqlServer2017SqlBuilder
{
	public SqlServer2019SqlBuilder(IDataProvider? provider, MappingSchema mappingSchema, ISqlOptimizer sqlOptimizer, SqlProviderFlags sqlProviderFlags)
		: base(provider, mappingSchema, sqlOptimizer, sqlProviderFlags)
	{
	}

	SqlServer2019SqlBuilder(BasicSqlBuilder parentBuilder) : base(parentBuilder)
	{
	}

	protected override ISqlBuilder CreateSqlBuilder()
	{
		return new SqlServer2019SqlBuilder(this);
	}

	public override string Name => ProviderName.SqlServer2019;
}
