using DBLayer.Data;
using DBLayer.Linq;
using DBLayer.SqlProvider;
using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace DBLayer.DataProvider.Firebird
{
	public interface IFirebirdExtensions
	{
	}

	public static class FirebirdExtensions
	{
		public static IFirebirdExtensions? Firebird(this Sql.ISqlExtension? ext) => null;

		[Sql.Extension("UUID_TO_CHAR({guid})", PreferServerSide = true, IsNullable = Sql.IsNullableType.SameAsFirstParameter)]
		public static string? UuidToChar(this IFirebirdExtensions? ext, [ExprParameter] Guid? guid) => guid?.ToString("D").ToUpperInvariant();
	}
}
