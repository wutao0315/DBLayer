﻿using DBLayer.Mapping;
using DBLayer.SqlProvider;
using DBLayer.SqlQuery;

namespace DBLayer.DataProvider.SqlServer;

class SqlServer2016SqlBuilder : SqlServer2014SqlBuilder
{
	public SqlServer2016SqlBuilder(IDataProvider? provider, MappingSchema mappingSchema, ISqlOptimizer sqlOptimizer, SqlProviderFlags sqlProviderFlags)
		: base(provider, mappingSchema, sqlOptimizer, sqlProviderFlags)
	{
	}

	protected SqlServer2016SqlBuilder(BasicSqlBuilder parentBuilder) : base(parentBuilder)
	{
	}

	protected override ISqlBuilder CreateSqlBuilder()
	{
		return new SqlServer2016SqlBuilder(this);
	}

	protected override void BuildDropTableStatement(SqlDropTableStatement dropTable)
	{
		BuildDropTableStatementIfExists(dropTable);
	}

	public override string Name => ProviderName.SqlServer2016;
}
