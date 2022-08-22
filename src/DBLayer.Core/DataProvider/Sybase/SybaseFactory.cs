using System.Collections.Generic;
using System.Linq;
using DBLayer.Configuration;


namespace DBLayer.DataProvider.Sybase;

class SybaseFactory : IDataProviderFactory
{
	IDataProvider IDataProviderFactory.GetDataProvider(IEnumerable<NamedValue> attributes)
	{
		var assemblyName = attributes.FirstOrDefault(_ => _.Name == "assemblyName");
		return SybaseTools.GetDataProvider(null, assemblyName?.Value);
	}
}
