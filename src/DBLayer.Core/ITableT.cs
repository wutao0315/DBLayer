﻿using DBLayer.Linq;

namespace DBLayer;
/// <summary>
/// Table-like queryable source, e.g. table, view or table-valued function.
/// </summary>
/// <typeparam name="T">Record mapping type.</typeparam>
public interface ITable<out T> : IExpressionQuery<T>
	where T : notnull
{
	string? ServerName { get; }
	string? DatabaseName { get; }
	string? SchemaName { get; }
	string TableName { get; }
	TableOptions TableOptions { get; }
	string? TableID { get; }
}
