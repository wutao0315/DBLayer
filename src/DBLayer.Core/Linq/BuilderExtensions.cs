using System.Reflection;

namespace DBLayer.Linq
{
	using Common;
	using DBLayer.Linq.Builder;

	internal static class BuilderExtensions
	{
		internal static SqlInfo[] Clone(this SqlInfo[] sqlInfos, MemberInfo member)
		{
			if (sqlInfos.Length == 0)
				return Array<SqlInfo>.Empty;

			var sql = new SqlInfo[sqlInfos.Length];
			for (var i = 0; i < sql.Length; i++)
				sql[i] = sqlInfos[i].Clone(member);
			return sql;
		}
	}
}
