﻿using DBLayer.Mapping;
using DBLayer.SqlProvider;
using System.Data.Common;

namespace DBLayer.DataProvider.Access;
class AccessOleDbSqlBuilder : AccessSqlBuilderBase
{
	public AccessOleDbSqlBuilder(IDataProvider? provider, MappingSchema mappingSchema, ISqlOptimizer sqlOptimizer, SqlProviderFlags sqlProviderFlags)
		: base(provider, mappingSchema, sqlOptimizer, sqlProviderFlags)
	{
	}

	AccessOleDbSqlBuilder(BasicSqlBuilder parentBuilder) : base(parentBuilder)
	{
	}

	protected override ISqlBuilder CreateSqlBuilder()
	{
		return new AccessOleDbSqlBuilder(this);
	}

	protected override string? GetProviderTypeName(IDataContext dataContext, DbParameter parameter)
	{
		if (DataProvider is AccessOleDbDataProvider provider)
		{
			var param = provider.TryGetProviderParameter(dataContext, parameter);
			if (param != null)
				return provider.Adapter.GetDbType(param).ToString();
		}

		return base.GetProviderTypeName(dataContext, parameter);
	}
}