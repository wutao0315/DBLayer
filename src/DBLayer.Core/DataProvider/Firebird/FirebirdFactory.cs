using System.Collections.Generic;
using DBLayer.Configuration;

namespace DBLayer.DataProvider.Firebird;

class FirebirdFactory: IDataProviderFactory
{
	IDataProvider IDataProviderFactory.GetDataProvider(IEnumerable<NamedValue> attributes)
	{
		return FirebirdTools.GetDataProvider();
	}
}
