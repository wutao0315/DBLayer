using System.Collections.Generic;
using DBLayer.Configuration;

namespace DBLayer.DataProvider.PostgreSQL;
class PostgreSQLFactory : IDataProviderFactory
{
	IDataProvider IDataProviderFactory.GetDataProvider(IEnumerable<NamedValue> attributes)
	{
		return PostgreSQLTools.GetDataProvider();
	}
}
