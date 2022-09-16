// ---------------------------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by DBLayer scaffolding tool (https://github.com/linq2db/linq2db).
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
// ---------------------------------------------------------------------------------------------------

using DBLayer.Mapping;

#pragma warning disable 1573, 1591
#nullable enable

namespace Cli.Default.MariaDB
{
	[Table("person")]
	public class Person
	{
		[Column("PersonID"  , IsPrimaryKey = true , IsIdentity = true, SkipOnInsert = true, SkipOnUpdate = true)] public int     PersonId   { get; set; } // int(11)
		[Column("FirstName" , CanBeNull    = false                                                             )] public string  FirstName  { get; set; } = null!; // varchar(50)
		[Column("LastName"  , CanBeNull    = false                                                             )] public string  LastName   { get; set; } = null!; // varchar(50)
		[Column("MiddleName"                                                                                   )] public string? MiddleName { get; set; } // varchar(50)
		[Column("Gender"                                                                                       )] public char    Gender     { get; set; } // char(1)

		#region Associations
		/// <summary>
		/// FK_Doctor_Person backreference
		/// </summary>
		[Association(ThisKey = nameof(PersonId), OtherKey = nameof(Doctor.PersonId))]
		public Doctor? DoctorPerson { get; set; }

		/// <summary>
		/// FK_Patient_Person backreference
		/// </summary>
		[Association(ThisKey = nameof(PersonId), OtherKey = nameof(Patient.PersonId))]
		public Patient? PatientPerson { get; set; }
		#endregion
	}
}
