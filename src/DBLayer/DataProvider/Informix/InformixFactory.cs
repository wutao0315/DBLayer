using DBLayer.Configuration;

namespace DBLayer.DataProvider.Informix;

class InformixFactory : IDataProviderFactory
{
	IDataProvider IDataProviderFactory.GetDataProvider(IEnumerable<NamedValue> attributes)
	{
		return InformixTools.GetDataProvider();
	}
}
