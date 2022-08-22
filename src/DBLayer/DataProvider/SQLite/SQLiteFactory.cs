using System.Collections.Generic;
using DBLayer.Configuration;

namespace DBLayer.DataProvider.SQLite;

class SQLiteFactory: IDataProviderFactory
{
	IDataProvider IDataProviderFactory.GetDataProvider(IEnumerable<NamedValue> attributes)
	{
		return SQLiteTools.GetDataProvider();
	}
}
