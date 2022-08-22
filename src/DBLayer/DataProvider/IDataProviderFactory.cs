using DBLayer.Configuration;

namespace DBLayer.DataProvider;

public interface IDataProviderFactory
{
	IDataProvider GetDataProvider (IEnumerable<NamedValue> attributes);
}
