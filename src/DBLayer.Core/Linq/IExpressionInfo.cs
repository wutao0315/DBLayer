using System.Linq.Expressions;

namespace DBLayer.Linq
{
	using DBLayer.Mapping;

	public interface IExpressionInfo
	{
		LambdaExpression GetExpression(MappingSchema mappingSchema);
	}
}
