using System.Diagnostics.CodeAnalysis;

namespace DBLayer.SqlQuery;

public interface IReadOnlyParameterValues
{
	bool TryGetValue(SqlParameter parameter, [NotNullWhen(true)] out SqlParameterValue? value);
}
