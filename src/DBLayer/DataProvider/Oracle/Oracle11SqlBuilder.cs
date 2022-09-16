﻿using DBLayer.Mapping;
using DBLayer.SqlProvider;
using DBLayer.SqlQuery;

namespace DBLayer.DataProvider.Oracle;

partial class Oracle11SqlBuilder : OracleSqlBuilderBase
{
	public Oracle11SqlBuilder(IDataProvider? provider, MappingSchema mappingSchema, ISqlOptimizer sqlOptimizer, SqlProviderFlags sqlProviderFlags)
		: base(provider, mappingSchema, sqlOptimizer, sqlProviderFlags)
	{
	}

	protected Oracle11SqlBuilder(BasicSqlBuilder parentBuilder) : base(parentBuilder)
	{
	}

	protected override ISqlBuilder CreateSqlBuilder()
	{
		return new Oracle11SqlBuilder(this) { HintBuilder = HintBuilder };
	}

	protected override string GetPhysicalTableName(ISqlTableSource table, string? alias, bool ignoreTableExpression = false, string? defaultDatabaseName = null)
	{
		var name = base.GetPhysicalTableName(table, alias, ignoreTableExpression, defaultDatabaseName);

		if (table.SqlTableType == SqlTableType.Function)
			return $"TABLE({name})";

		return name;
	}
}