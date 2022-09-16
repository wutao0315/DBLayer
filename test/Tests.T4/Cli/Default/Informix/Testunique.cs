// ---------------------------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by DBLayer scaffolding tool (https://github.com/linq2db/linq2db).
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
// ---------------------------------------------------------------------------------------------------

using DBLayer.Mapping;
using System.Collections.Generic;

#pragma warning disable 1573, 1591
#nullable enable

namespace Cli.Default.Informix
{
	[Table("testunique")]
	public class Testunique
	{
		[Column("id1", IsPrimaryKey = true, PrimaryKeyOrder = 0)] public int Id1 { get; set; } // INTEGER
		[Column("id2", IsPrimaryKey = true, PrimaryKeyOrder = 1)] public int Id2 { get; set; } // INTEGER
		[Column("id3"                                          )] public int Id3 { get; set; } // INTEGER
		[Column("id4"                                          )] public int Id4 { get; set; } // INTEGER

		#region Associations
		/// <summary>
		/// FK_testfkunique_testunique backreference
		/// </summary>
		[Association(ThisKey = nameof(Id1) + "," + nameof(Id1), OtherKey = nameof(Testfkunique.Id1) + "," + nameof(Id1))]
		public IEnumerable<Testfkunique> Testfkuniques { get; set; } = null!;

		/// <summary>
		/// FK_testfkunique_testunique_1 backreference
		/// </summary>
		[Association(ThisKey = nameof(Id3) + "," + nameof(Id3), OtherKey = nameof(Testfkunique.Id3) + "," + nameof(Id3))]
		public IEnumerable<Testfkunique> Testfkuniques1 { get; set; } = null!;
		#endregion
	}
}
