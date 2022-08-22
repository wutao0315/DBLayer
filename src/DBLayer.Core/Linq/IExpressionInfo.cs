using System.Linq.Expressions;

namespace DBLayer.Linq
{
	using Mapping;

	public interface IExpressionInfo
	{
		LambdaExpression GetExpression(MappingSchema mappingSchema);
	}
}
