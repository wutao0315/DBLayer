using System.Linq.Expressions;

namespace DBLayer.Linq;

public interface IExpressionQuery
{
	Expression   Expression  { get; }
	string       SqlText     { get; }
	IDataContext DataContext { get; }
}
