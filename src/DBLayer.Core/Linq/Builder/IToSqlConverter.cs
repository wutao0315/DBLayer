using System.Linq.Expressions;

using DBLayer.SqlQuery;

namespace DBLayer.Linq.Builder
{
	public interface IToSqlConverter
	{
		ISqlExpression ToSql(Expression expression);
	}
}
