// ---------------------------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by DBLayer scaffolding tool (https://github.com/linq2db/linq2db).
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
// ---------------------------------------------------------------------------------------------------

using DBLayer.Mapping;
using System;
using System.Collections.Generic;

#pragma warning disable 1573, 1591
#nullable enable

namespace Cli.Default.SqlServerNorthwind
{
	[Table("Employees")]
	public class Employee
	{
		[Column("EmployeeID"     , IsPrimaryKey = true , IsIdentity = true, SkipOnInsert = true, SkipOnUpdate = true)] public int       EmployeeId      { get; set; } // int
		[Column("LastName"       , CanBeNull    = false                                                             )] public string    LastName        { get; set; } = null!; // nvarchar(20)
		[Column("FirstName"      , CanBeNull    = false                                                             )] public string    FirstName       { get; set; } = null!; // nvarchar(10)
		[Column("Title"                                                                                             )] public string?   Title           { get; set; } // nvarchar(30)
		[Column("TitleOfCourtesy"                                                                                   )] public string?   TitleOfCourtesy { get; set; } // nvarchar(25)
		[Column("BirthDate"                                                                                         )] public DateTime? BirthDate       { get; set; } // datetime
		[Column("HireDate"                                                                                          )] public DateTime? HireDate        { get; set; } // datetime
		[Column("Address"                                                                                           )] public string?   Address         { get; set; } // nvarchar(60)
		[Column("City"                                                                                              )] public string?   City            { get; set; } // nvarchar(15)
		[Column("Region"                                                                                            )] public string?   Region          { get; set; } // nvarchar(15)
		[Column("PostalCode"                                                                                        )] public string?   PostalCode      { get; set; } // nvarchar(10)
		[Column("Country"                                                                                           )] public string?   Country         { get; set; } // nvarchar(15)
		[Column("HomePhone"                                                                                         )] public string?   HomePhone       { get; set; } // nvarchar(24)
		[Column("Extension"                                                                                         )] public string?   Extension       { get; set; } // nvarchar(4)
		[Column("Photo"                                                                                             )] public byte[]?   Photo           { get; set; } // image
		[Column("Notes"                                                                                             )] public string?   Notes           { get; set; } // ntext
		[Column("ReportsTo"                                                                                         )] public int?      ReportsTo       { get; set; } // int
		[Column("PhotoPath"                                                                                         )] public string?   PhotoPath       { get; set; } // nvarchar(255)

		#region Associations
		/// <summary>
		/// FK_Employees_Employees
		/// </summary>
		[Association(ThisKey = nameof(ReportsTo), OtherKey = nameof(EmployeeId))]
		public Employee? Employees { get; set; }

		/// <summary>
		/// FK_Employees_Employees backreference
		/// </summary>
		[Association(ThisKey = nameof(EmployeeId), OtherKey = nameof(ReportsTo))]
		public IEnumerable<Employee> Employees1 { get; set; } = null!;

		/// <summary>
		/// FK_Orders_Employees backreference
		/// </summary>
		[Association(ThisKey = nameof(EmployeeId), OtherKey = nameof(Order.EmployeeId))]
		public IEnumerable<Order> Orders { get; set; } = null!;

		/// <summary>
		/// FK_EmployeeTerritories_Employees backreference
		/// </summary>
		[Association(ThisKey = nameof(EmployeeId), OtherKey = nameof(EmployeeTerritory.EmployeeId))]
		public IEnumerable<EmployeeTerritory> EmployeeTerritories { get; set; } = null!;
		#endregion
	}
}