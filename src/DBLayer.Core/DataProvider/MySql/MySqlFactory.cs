using JetBrains.Annotations;

namespace DBLayer.DataProvider.MySql
{
	using System.Collections.Generic;
	using Configuration;

	[UsedImplicitly]
	class MySqlFactory : IDataProviderFactory
	{
		IDataProvider IDataProviderFactory.GetDataProvider(IEnumerable<NamedValue> attributes)
		{
			return MySqlTools.GetDataProvider();
		}
	}
}
