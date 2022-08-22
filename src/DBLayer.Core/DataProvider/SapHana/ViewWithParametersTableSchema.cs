using System.Collections.Generic;
using DBLayer.SchemaProvider;

namespace DBLayer.DataProvider.SapHana;

public class ViewWithParametersTableSchema : TableSchema
{
	public ViewWithParametersTableSchema()
	{
		IsProviderSpecific = true;
	}

	public List<ParameterSchema>? Parameters { get; set; }
}
