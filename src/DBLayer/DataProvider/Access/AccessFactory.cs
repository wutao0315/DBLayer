using DBLayer.Configuration;

namespace DBLayer.DataProvider.Access;

class AccessFactory : IDataProviderFactory
{
	IDataProvider IDataProviderFactory.GetDataProvider(IEnumerable<NamedValue> attributes)
	{
		return AccessTools.GetDataProvider();
	}
}
