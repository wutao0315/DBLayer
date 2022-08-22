﻿using System.Collections.Generic;

using JetBrains.Annotations;

namespace DBLayer.DataProvider.PostgreSQL
{
	using Configuration;

	[UsedImplicitly]
	class PostgreSQLFactory : IDataProviderFactory
	{
		IDataProvider IDataProviderFactory.GetDataProvider(IEnumerable<NamedValue> attributes)
		{
			return PostgreSQLTools.GetDataProvider();
		}
	}
}
