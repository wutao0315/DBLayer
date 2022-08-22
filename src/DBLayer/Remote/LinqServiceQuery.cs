using DBLayer.SqlQuery;

namespace DBLayer.Remote;

public class LinqServiceQuery
{
	public SqlStatement                 Statement  { get; set; } = null!;
	public IReadOnlyCollection<string>? QueryHints { get; set; }
}
