// ---------------------------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by DBLayer scaffolding tool (https://github.com/linq2db/linq2db).
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
// ---------------------------------------------------------------------------------------------------

using DBLayer.Mapping;

#pragma warning disable 1573, 1591
#nullable enable

namespace Cli.Default.MariaDB
{
	[Table("fulltextindextest")]
	public class Fulltextindextest
	{
		[Column("id"        , IsPrimaryKey = true, IsIdentity = true, SkipOnInsert = true, SkipOnUpdate = true)] public uint    Id         { get; set; } // int(10) unsigned
		[Column("TestField1"                                                                                  )] public string? TestField1 { get; set; } // text
		[Column("TestField2"                                                                                  )] public string? TestField2 { get; set; } // text
	}
}