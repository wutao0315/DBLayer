using DBLayer.SqlQuery;

namespace DBLayer.Remote;

interface IQueryExtendible
{
	List<SqlQueryExtension>? SqlQueryExtensions { get; set; }
}
