// ---------------------------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by DBLayer scaffolding tool (https://github.com/linq2db/linq2db).
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
// ---------------------------------------------------------------------------------------------------

using DBLayer.Mapping;

#pragma warning disable 1573, 1591
#nullable enable

namespace Cli.Default.Firebird
{
	[Table("TestIdentity")]
	public class TestIdentity
	{
		[Column("ID", IsPrimaryKey = true)] public int Id { get; set; } // integer
	}
}
