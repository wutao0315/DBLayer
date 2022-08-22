using System.Linq.Expressions;

namespace DBLayer.Expressions
{
	public interface ICustomMapper
	{
		bool CanMap(Expression expression);
		Expression Map(Expression expression);
	}
}
