// ---------------------------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by DBLayer scaffolding tool (https://github.com/linq2db/linq2db).
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
// ---------------------------------------------------------------------------------------------------

using DBLayer.Mapping;

#pragma warning disable 1573, 1591
#nullable enable

namespace Cli.Default.Access.OleDb
{
	[Table("Patient_SelectAll", IsView = true)]
	public class PatientSelectAll
	{
		[Column("PersonID"  )] public int     PersonId   { get; set; } // Long
		[Column("FirstName" )] public string? FirstName  { get; set; } // VarChar(50)
		[Column("LastName"  )] public string? LastName   { get; set; } // VarChar(50)
		[Column("MiddleName")] public string? MiddleName { get; set; } // VarChar(50)
		[Column("Gender"    )] public char?   Gender     { get; set; } // VarChar(1)
		[Column("Diagnosis" )] public string? Diagnosis  { get; set; } // VarChar(255)
	}
}
