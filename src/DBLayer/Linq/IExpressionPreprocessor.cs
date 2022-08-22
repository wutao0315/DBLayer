using System.Linq.Expressions;

namespace DBLayer.Linq;

public interface IExpressionPreprocessor
{
	Expression ProcessExpression(Expression expression);
}
