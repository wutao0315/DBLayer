using System;
using System.Data;

namespace DBLayer.Benchmarks.TestProvider
{
	public class QueryResult
	{
		public DataTable?   Schema     = null!;

		public string[]?    Names      = null!;
		public string[]?    DbTypes    = null!;
		public Type[]?      FieldTypes = null!;

		public object?[][]? Data       = null!;

		public int          Return;

		public Func<string, bool>? Match;
	}
}
