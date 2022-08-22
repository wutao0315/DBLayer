using System.Collections.Generic;
using System.Linq;
using DBLayer.Configuration;

namespace DBLayer.DataProvider.SapHana;

class SapHanaFactory : IDataProviderFactory
{
	IDataProvider IDataProviderFactory.GetDataProvider(IEnumerable<NamedValue> attributes)
	{
		var assemblyName = attributes.FirstOrDefault(_ => _.Name == "assemblyName");
		return SapHanaTools.GetDataProvider(null, assemblyName?.Value);
	}
}
