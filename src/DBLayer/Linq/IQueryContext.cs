using DBLayer.SqlQuery;

namespace DBLayer.Linq;

public interface IQueryContext
{
	SqlStatement    Statement   { get; }
	object?         Context     { get; set; }

	SqlParameter[]? Parameters  { get; set; }
	AliasesContext? Aliases     { get; set; }
}
