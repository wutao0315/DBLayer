using DBLayer.SqlQuery;
using System.Text;

namespace DBLayer.SqlProvider;

class HintExtensionBuilder : ISqlQueryExtensionBuilder
{
	void ISqlQueryExtensionBuilder.Build(ISqlBuilder sqlBuilder, StringBuilder stringBuilder, SqlQueryExtension sqlQueryExtension)
	{
		var hint = (SqlValue)sqlQueryExtension.Arguments["hint"];
		stringBuilder.Append((string)hint.Value!);
	}
}
