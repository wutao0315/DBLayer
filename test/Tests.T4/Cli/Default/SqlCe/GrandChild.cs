// ---------------------------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by DBLayer scaffolding tool (https://github.com/linq2db/linq2db).
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
// ---------------------------------------------------------------------------------------------------

using DBLayer.Mapping;

#pragma warning disable 1573, 1591
#nullable enable

namespace Cli.Default.SqlCe
{
	[Table("GrandChild")]
	public class GrandChild
	{
		[Column("ParentID"    )] public int? ParentId     { get; set; } // int
		[Column("ChildID"     )] public int? ChildId      { get; set; } // int
		[Column("GrandChildID")] public int? GrandChildId { get; set; } // int
	}
}
