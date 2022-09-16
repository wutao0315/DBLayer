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

namespace Cli.Default.SQLiteNorthwind
{
	[Table("Employees")]
	public class Employee
	{
		[Column("EmployeeID"     , IsPrimaryKey = true                      )] public int       EmployeeId      { get; set; } // int
		[Column("LastName"       , CanBeNull    = false                     )] public string    LastName        { get; set; } = null!; // varchar(20)
		[Column("FirstName"      , CanBeNull    = false                     )] public string    FirstName       { get; set; } = null!; // varchar(10)
		[Column("Title"                                                     )] public string?   Title           { get; set; } // varchar(30)
		[Column("TitleOfCourtesy"                                           )] public string?   TitleOfCourtesy { get; set; } // varchar(25)
		[Column("BirthDate"      , SkipOnInsert = true , SkipOnUpdate = true)] public DateTime? BirthDate       { get; set; } // timestamp
		[Column("HireDate"       , SkipOnInsert = true , SkipOnUpdate = true)] public DateTime? HireDate        { get; set; } // timestamp
		[Column("Address"                                                   )] public string?   Address         { get; set; } // varchar(60)
		[Column("City"                                                      )] public string?   City            { get; set; } // varchar(15)
		[Column("Region"                                                    )] public string?   Region          { get; set; } // varchar(15)
		[Column("PostalCode"                                                )] public string?   PostalCode      { get; set; } // varchar(10)
		[Column("Country"                                                   )] public string?   Country         { get; set; } // varchar(15)
		[Column("HomePhone"                                                 )] public string?   HomePhone       { get; set; } // varchar(24)
		[Column("Extension"                                                 )] public string?   Extension       { get; set; } // varchar(4)
		[Column("Photo"                                                     )] public byte[]?   Photo           { get; set; } // blob
		[Column("Notes"                                                     )] public string?   Notes           { get; set; } // text(max)
		[Column("ReportsTo"                                                 )] public int?      ReportsTo       { get; set; } // int
		[Column("PhotoPath"                                                 )] public string?   PhotoPath       { get; set; } // varchar(255)
	}
}
