using System.Linq.Expressions;
using DBLayer.Mapping;

namespace DBLayer.Linq;

public interface IExpressionInfo
{
	LambdaExpression GetExpression(MappingSchema mappingSchema);
}
