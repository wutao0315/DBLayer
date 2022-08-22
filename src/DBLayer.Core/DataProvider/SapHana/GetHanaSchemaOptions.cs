using DBLayer.Data;
using DBLayer.SchemaProvider;

namespace DBLayer.DataProvider.SapHana;
public class GetHanaSchemaOptions: GetSchemaOptions
{
	public Func<ProcedureSchema, DataParameter[]?> GetStoredProcedureParameters = schema => null;
	public bool ThrowExceptionIfCalculationViewsNotAuthorized;
}
