// ---------------------------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by DBLayer scaffolding tool (https://github.com/linq2db/linq2db).
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
// ---------------------------------------------------------------------------------------------------

using DBLayer.Mapping;

#pragma warning disable 1573, 1591
#nullable enable

namespace Cli.Default.SQLite
{
	[Table("TEST_T4_CASING")]
	public class TestT4Casing
	{
		[Column("ALL_CAPS"             )] public int AllCaps             { get; set; } // int
		[Column("CAPS"                 )] public int Caps                { get; set; } // int
		[Column("PascalCase"           )] public int PascalCase          { get; set; } // int
		[Column("Pascal_Snake_Case"    )] public int PascalSnakeCase     { get; set; } // int
		[Column("PascalCase_Snake_Case")] public int PascalCaseSnakeCase { get; set; } // int
		[Column("snake_case"           )] public int SnakeCase           { get; set; } // int
		[Column("camelCase"            )] public int CamelCase           { get; set; } // int
	}
}
