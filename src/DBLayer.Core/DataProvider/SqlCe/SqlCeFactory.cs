using System.Collections.Generic;
using DBLayer.Configuration;

namespace DBLayer.DataProvider.SqlCe;

class SqlCeFactory : IDataProviderFactory
{
	IDataProvider IDataProviderFactory.GetDataProvider(IEnumerable<NamedValue> attributes)
	{
		return SqlCeTools.GetDataProvider();
	}
}
