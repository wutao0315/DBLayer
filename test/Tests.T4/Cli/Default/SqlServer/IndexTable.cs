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
	[Table("IndexTable")]
	public class IndexTable
	{
		[Column("PKField1"   , IsPrimaryKey = true, PrimaryKeyOrder = 1)] public int PkField1    { get; set; } // int
		[Column("PKField2"   , IsPrimaryKey = true, PrimaryKeyOrder = 0)] public int PkField2    { get; set; } // int
		[Column("UniqueField"                                          )] public int UniqueField { get; set; } // int
		[Column("IndexField"                                           )] public int IndexField  { get; set; } // int

		#region Associations
		/// <summary>
		/// FK_Patient2_IndexTable backreference
		/// </summary>
		[Association(ThisKey = nameof(PkField2) + "," + nameof(PkField2), OtherKey = nameof(IndexTable2.PkField2) + "," + nameof(PkField2))]
		public IndexTable2? Patient { get; set; }
		#endregion
	}
}
