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
	[Table("doctor")]
	public class Doctor
	{
		[Column("personid", IsPrimaryKey = true )] public int    Personid { get; set; } // INTEGER
		[Column("taxonomy", CanBeNull    = false)] public string Taxonomy { get; set; } = null!; // NVARCHAR(50)

		#region Associations
		/// <summary>
		/// FK_doctor_person
		/// </summary>
		[Association(CanBeNull = false, ThisKey = nameof(Personid), OtherKey = nameof(Informix.Person.Personid))]
		public Person Person { get; set; } = null!;
		#endregion
	}
}
