// ---------------------------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by DBLayer scaffolding tool (https://github.com/linq2db/linq2db).
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
// ---------------------------------------------------------------------------------------------------

using DBLayer.Mapping;
using System;

#pragma warning disable 1573, 1591
#nullable enable

namespace Cli.Default.SqlServer
{
	[Table("GuidID")]
	public class GuidId
	{
		[Column("ID"    , IsPrimaryKey = true)] public Guid Id     { get; set; } // uniqueidentifier
		[Column("Field1"                     )] public int? Field1 { get; set; } // int
	}
}