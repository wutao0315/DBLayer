using DBLayer.Linq;
using DBLayer.SqlProvider;
using System.Linq.Expressions;


namespace DBLayer.DataProvider.SqlServer;

public interface ISqlServerSpecificTable<out TSource> : ITable<TSource>
	where TSource : notnull
{
}

class SqlServerSpecificTable<TSource> : DatabaseSpecificTable<TSource>, ISqlServerSpecificTable<TSource>, ITable
	where TSource : notnull
{
	public SqlServerSpecificTable(ITable<TSource> table) : base(table)
	{
	}
}

public interface ISqlServerSpecificQueryable<out TSource> : IQueryable<TSource>
{
}

class SqlServerSpecificQueryable<TSource> : DatabaseSpecificQueryable<TSource>, ISqlServerSpecificQueryable<TSource>, ITable
{
	public SqlServerSpecificQueryable(IQueryable<TSource> queryable) : base(queryable)
	{
	}
}

public static partial class SqlServerTools
{
	
	[DBLayer.Sql.QueryExtension(null, DBLayer.Sql.QueryExtensionScope.None, typeof(NoneExtensionBuilder))]
	public static ISqlServerSpecificTable<TSource> AsSqlServer<TSource>(this ITable<TSource> table)
		where TSource : notnull
	{
		table.Expression = Expression.Call(
			null,
			MethodHelper.GetMethodInfo(AsSqlServer, table),
			table.Expression);

		return new SqlServerSpecificTable<TSource>(table);
	}

	
	[DBLayer.Sql.QueryExtension(null, DBLayer.Sql.QueryExtensionScope.None, typeof(NoneExtensionBuilder))]
	public static ISqlServerSpecificQueryable<TSource> AsSqlServer<TSource>(this IQueryable<TSource> source)
		where TSource : notnull
	{
		var currentSource = LinqExtensions.ProcessSourceQueryable?.Invoke(source) ?? source;

		return new SqlServerSpecificQueryable<TSource>(currentSource.Provider.CreateQuery<TSource>(
			Expression.Call(
				null,
				MethodHelper.GetMethodInfo(AsSqlServer, source),
				currentSource.Expression)));
	}
}
