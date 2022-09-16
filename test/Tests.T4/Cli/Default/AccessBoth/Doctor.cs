// ---------------------------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by DBLayer scaffolding tool (https://github.com/linq2db/linq2db).
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
// ---------------------------------------------------------------------------------------------------

using DBLayer.Mapping;

#pragma warning disable 1573, 1591
#nullable enable

namespace Cli.Default.Access.Both
{
	[Table("Doctor")]
	public class Doctor
	{
		[Column("PersonID", IsPrimaryKey = true )] public int    PersonId { get; set; } // Long
		[Column("Taxonomy", CanBeNull    = false)] public string Taxonomy { get; set; } = null!; // VarChar(50)

		#region Associations
		/// <summary>
		/// PersonDoctor
		/// </summary>
		[Association(CanBeNull = false, ThisKey = nameof(PersonId), OtherKey = nameof(Both.Person.PersonId))]
		public Person Person { get; set; } = null!;
		#endregion
	}
}
