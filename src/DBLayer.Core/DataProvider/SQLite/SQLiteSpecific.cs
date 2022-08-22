using System;
using System.Linq.Expressions;
using DBLayer.Linq;
using DBLayer.SqlProvider;

namespace DBLayer.DataProvider.SQLite;

public interface ISQLiteSpecificTable<out TSource> : ITable<TSource>
	where TSource : notnull
{
}

class SQLiteSpecificTable<TSource> : DatabaseSpecificTable<TSource>, ISQLiteSpecificTable<TSource>, ITable
	where TSource : notnull
{
	public SQLiteSpecificTable(ITable<TSource> table) : base(table)
	{
	}
}

public static partial class SQLiteTools
{
	
	[Sql.QueryExtension(null, Sql.QueryExtensionScope.None, typeof(NoneExtensionBuilder))]
	public static ISQLiteSpecificTable<TSource> AsSQLite<TSource>(this ITable<TSource> table)
		where TSource : notnull
	{
		table.Expression = Expression.Call(
			null,
			MethodHelper.GetMethodInfo(AsSQLite, table),
			table.Expression);

		return new SQLiteSpecificTable<TSource>(table);
	}
}
