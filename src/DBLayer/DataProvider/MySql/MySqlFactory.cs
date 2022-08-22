using System.Collections.Generic;
using DBLayer.Configuration;

namespace DBLayer.DataProvider.MySql;

class MySqlFactory : IDataProviderFactory
{
	IDataProvider IDataProviderFactory.GetDataProvider(IEnumerable<NamedValue> attributes)
	{
		return MySqlTools.GetDataProvider();
	}
}
