using System;

namespace DBLayer.DataProvider.SapHana
{
	using Data;
	using SchemaProvider;

	public class GetHanaSchemaOptions: GetSchemaOptions
	{
		public Func<ProcedureSchema, DataParameter[]?> GetStoredProcedureParameters = schema => null;
		public bool ThrowExceptionIfCalculationViewsNotAuthorized;
	}
}
