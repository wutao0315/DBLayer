// ---------------------------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by DBLayer scaffolding tool (https://github.com/linq2db/linq2db).
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
// ---------------------------------------------------------------------------------------------------

using DBLayer.Mapping;

#pragma warning disable 1573, 1591
#nullable enable

namespace Cli.Default.Informix
{
	[Table("grandchild")]
	public class Grandchild
	{
		[Column("parentid"    )] public int? Parentid     { get; set; } // INTEGER
		[Column("childid"     )] public int? Childid      { get; set; } // INTEGER
		[Column("grandchildid")] public int? Grandchildid { get; set; } // INTEGER
	}
}