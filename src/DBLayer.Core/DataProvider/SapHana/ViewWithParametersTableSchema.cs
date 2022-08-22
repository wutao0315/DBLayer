using System.Collections.Generic;

namespace DBLayer.DataProvider.SapHana
{
	using SchemaProvider;


	public class ViewWithParametersTableSchema : TableSchema
	{
		public ViewWithParametersTableSchema()
		{
			IsProviderSpecific = true;
		}

		public List<ParameterSchema>? Parameters { get; set; }
	}
}
