// ---------------------------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by DBLayer scaffolding tool (https://github.com/linq2db/linq2db).
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
// ---------------------------------------------------------------------------------------------------

using DBLayer.Mapping;

#pragma warning disable 1573, 1591
#nullable enable

namespace Cli.Default.SqlServer
{
	[Table("ParentView", IsView = true)]
	public class ParentView
	{
		[Column("ParentID"                                                             )] public int? ParentId { get; set; } // int
		[Column("Value1"                                                               )] public int? Value1   { get; set; } // int
		[Column("_ID"     , IsIdentity = true, SkipOnInsert = true, SkipOnUpdate = true)] public int  Id       { get; set; } // int
	}
}
