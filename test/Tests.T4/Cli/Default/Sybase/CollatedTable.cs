// ---------------------------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by DBLayer scaffolding tool (https://github.com/linq2db/linq2db).
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
// ---------------------------------------------------------------------------------------------------

using DBLayer.Mapping;

#pragma warning disable 1573, 1591
#nullable enable

namespace Cli.Default.Sybase
{
	[Table("CollatedTable")]
	public class CollatedTable
	{
		[Column("Id"                                )] public int    Id              { get; set; } // int
		[Column("CaseSensitive"  , CanBeNull = false)] public string CaseSensitive   { get; set; } = null!; // nvarchar(60)
		[Column("CaseInsensitive", CanBeNull = false)] public string CaseInsensitive { get; set; } = null!; // nvarchar(60)
	}
}
