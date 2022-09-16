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
	[Table("Orders")]
	public class Order
	{
		[Column("OrderID"       , IsPrimaryKey = true, IsIdentity = true, SkipOnInsert = true, SkipOnUpdate = true)] public int       OrderId        { get; set; } // int
		[Column("CustomerID"                                                                                      )] public string?   CustomerId     { get; set; } // nchar(5)
		[Column("EmployeeID"                                                                                      )] public int?      EmployeeId     { get; set; } // int
		[Column("OrderDate"                                                                                       )] public DateTime? OrderDate      { get; set; } // datetime
		[Column("RequiredDate"                                                                                    )] public DateTime? RequiredDate   { get; set; } // datetime
		[Column("ShippedDate"                                                                                     )] public DateTime? ShippedDate    { get; set; } // datetime
		[Column("ShipVia"                                                                                         )] public int?      ShipVia        { get; set; } // int
		[Column("Freight"                                                                                         )] public decimal?  Freight        { get; set; } // money
		[Column("ShipName"                                                                                        )] public string?   ShipName       { get; set; } // nvarchar(40)
		[Column("ShipAddress"                                                                                     )] public string?   ShipAddress    { get; set; } // nvarchar(60)
		[Column("ShipCity"                                                                                        )] public string?   ShipCity       { get; set; } // nvarchar(15)
		[Column("ShipRegion"                                                                                      )] public string?   ShipRegion     { get; set; } // nvarchar(15)
		[Column("ShipPostalCode"                                                                                  )] public string?   ShipPostalCode { get; set; } // nvarchar(10)
		[Column("ShipCountry"                                                                                     )] public string?   ShipCountry    { get; set; } // nvarchar(15)

		#region Associations
		/// <summary>
		/// FK_Orders_Employees
		/// </summary>
		[Association(ThisKey = nameof(EmployeeId), OtherKey = nameof(SqlServerNorthwind.Employee.EmployeeId))]
		public Employee? Employee { get; set; }

		/// <summary>
		/// FK_Orders_Customers
		/// </summary>
		[Association(ThisKey = nameof(CustomerId), OtherKey = nameof(SqlServerNorthwind.Customer.CustomerId))]
		public Customer? Customer { get; set; }

		/// <summary>
		/// FK_Orders_Shippers
		/// </summary>
		[Association(ThisKey = nameof(ShipVia), OtherKey = nameof(Shipper.ShipperId))]
		public Shipper? Shippers { get; set; }

		/// <summary>
		/// FK_Order_Details_Orders backreference
		/// </summary>
		[Association(ThisKey = nameof(OrderId), OtherKey = nameof(OrderDetail.OrderId))]
		public IEnumerable<OrderDetail> OrderDetails { get; set; } = null!;
		#endregion
	}
}
