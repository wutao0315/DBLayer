﻿using DBLayer.Configuration;

namespace DBLayer.DataProvider.SqlServer;

class SqlServerFactory : IDataProviderFactory
{
	IDataProvider IDataProviderFactory.GetDataProvider(IEnumerable<NamedValue> attributes)
	{
		var provider     = SqlServerProvider.SystemDataSqlClient;
		var version      = attributes.FirstOrDefault(_ => _.Name == "version")?.Value;
		var assemblyName = attributes.FirstOrDefault(_ => _.Name == "assemblyName")?.Value;

		if (assemblyName == SqlServerProviderAdapter.MicrosoftAssemblyName)
		{
			provider = SqlServerProvider.MicrosoftDataSqlClient;
		}

		return version switch
		{
			"2005" => SqlServerTools.GetDataProvider(SqlServerVersion.v2005, provider),
			"2012" => SqlServerTools.GetDataProvider(SqlServerVersion.v2012, provider),
			"2014" => SqlServerTools.GetDataProvider(SqlServerVersion.v2014, provider),
			"2016" => SqlServerTools.GetDataProvider(SqlServerVersion.v2016, provider),
			"2017" => SqlServerTools.GetDataProvider(SqlServerVersion.v2017, provider),
			"2019" => SqlServerTools.GetDataProvider(SqlServerVersion.v2019, provider),
			_      => SqlServerTools.GetDataProvider(SqlServerVersion.v2008, provider),
		};
	}
}
