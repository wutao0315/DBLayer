﻿//---------------------------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated by T4Model template for T4 (https://github.com/linq2db/linq2db).
//    Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//---------------------------------------------------------------------------------------------------

#pragma warning disable 1573, 1591
#nullable enable

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using DBLayer;
using DBLayer.Common;
using DBLayer.Configuration;
using DBLayer.Data;
using DBLayer.Mapping;

namespace Default.SqlServerNorthwind
{
	public partial class TestDataDB : DBLayer.Data.DataConnection
	{
		public ITable<AlphabeticalListOfProduct>  AlphabeticalListOfProducts   { get { return this.GetTable<AlphabeticalListOfProduct>(); } }
		public ITable<Category>                   Categories                   { get { return this.GetTable<Category>(); } }
		public ITable<CategorySalesFor1997>       CategorySalesFor1997         { get { return this.GetTable<CategorySalesFor1997>(); } }
		public ITable<CurrentProductList>         CurrentProductLists          { get { return this.GetTable<CurrentProductList>(); } }
		public ITable<Customer>                   Customers                    { get { return this.GetTable<Customer>(); } }
		public ITable<CustomerAndSuppliersByCity> CustomerAndSuppliersByCities { get { return this.GetTable<CustomerAndSuppliersByCity>(); } }
		public ITable<CustomerCustomerDemo>       CustomerCustomerDemoes       { get { return this.GetTable<CustomerCustomerDemo>(); } }
		public ITable<CustomerDemographic>        CustomerDemographics         { get { return this.GetTable<CustomerDemographic>(); } }
		public ITable<Employee>                   Employees                    { get { return this.GetTable<Employee>(); } }
		public ITable<EmployeeTerritory>          EmployeeTerritories          { get { return this.GetTable<EmployeeTerritory>(); } }
		public ITable<Invoice>                    Invoices                     { get { return this.GetTable<Invoice>(); } }
		public ITable<Order>                      Orders                       { get { return this.GetTable<Order>(); } }
		public ITable<OrderDetail>                OrderDetails                 { get { return this.GetTable<OrderDetail>(); } }
		public ITable<OrderDetailsExtended>       OrderDetailsExtendeds        { get { return this.GetTable<OrderDetailsExtended>(); } }
		public ITable<OrdersQry>                  OrdersQries                  { get { return this.GetTable<OrdersQry>(); } }
		public ITable<OrderSubtotal>              OrderSubtotals               { get { return this.GetTable<OrderSubtotal>(); } }
		public ITable<Product>                    Products                     { get { return this.GetTable<Product>(); } }
		public ITable<ProductsAboveAveragePrice>  ProductsAboveAveragePrices   { get { return this.GetTable<ProductsAboveAveragePrice>(); } }
		public ITable<ProductSalesFor1997>        ProductSalesFor1997          { get { return this.GetTable<ProductSalesFor1997>(); } }
		public ITable<ProductsByCategory>         ProductsByCategories         { get { return this.GetTable<ProductsByCategory>(); } }
		public ITable<QuarterlyOrder>             QuarterlyOrders              { get { return this.GetTable<QuarterlyOrder>(); } }
		public ITable<Region>                     Regions                      { get { return this.GetTable<Region>(); } }
		public ITable<SalesByCategory>            SalesByCategories            { get { return this.GetTable<SalesByCategory>(); } }
		public ITable<SalesTotalsByAmount>        SalesTotalsByAmounts         { get { return this.GetTable<SalesTotalsByAmount>(); } }
		public ITable<Shipper>                    Shippers                     { get { return this.GetTable<Shipper>(); } }
		public ITable<SummaryOfSalesByQuarter>    SummaryOfSalesByQuarters     { get { return this.GetTable<SummaryOfSalesByQuarter>(); } }
		public ITable<SummaryOfSalesByYear>       SummaryOfSalesByYears        { get { return this.GetTable<SummaryOfSalesByYear>(); } }
		public ITable<Supplier>                   Suppliers                    { get { return this.GetTable<Supplier>(); } }
		public ITable<Territory>                  Territories                  { get { return this.GetTable<Territory>(); } }

		public TestDataDB()
		{
			InitDataContext();
			InitMappingSchema();
		}

		public TestDataDB(string configuration)
			: base(configuration)
		{
			InitDataContext();
			InitMappingSchema();
		}

		public TestDataDB(DBLayerConnectionOptions options)
			: base(options)
		{
			InitDataContext();
			InitMappingSchema();
		}

		public TestDataDB(DBLayerConnectionOptions<TestDataDB> options)
			: base(options)
		{
			InitDataContext();
			InitMappingSchema();
		}

		partial void InitDataContext  ();
		partial void InitMappingSchema();
	}

	[Table(Schema="dbo", Name="Alphabetical list of products", IsView=true)]
	public partial class AlphabeticalListOfProduct
	{
		[Column, NotNull    ] public int      ProductID       { get; set; } // int
		[Column, NotNull    ] public string   ProductName     { get; set; } = null!; // nvarchar(40)
		[Column,    Nullable] public int?     SupplierID      { get; set; } // int
		[Column,    Nullable] public int?     CategoryID      { get; set; } // int
		[Column,    Nullable] public string?  QuantityPerUnit { get; set; } // nvarchar(20)
		[Column,    Nullable] public decimal? UnitPrice       { get; set; } // money
		[Column,    Nullable] public short?   UnitsInStock    { get; set; } // smallint
		[Column,    Nullable] public short?   UnitsOnOrder    { get; set; } // smallint
		[Column,    Nullable] public short?   ReorderLevel    { get; set; } // smallint
		[Column, NotNull    ] public bool     Discontinued    { get; set; } // bit
		[Column, NotNull    ] public string   CategoryName    { get; set; } = null!; // nvarchar(15)
	}

	[Table(Schema="dbo", Name="Categories")]
	public partial class Category
	{
		[PrimaryKey, Identity   ] public int     CategoryID   { get; set; } // int
		[Column,     NotNull    ] public string  CategoryName { get; set; } = null!; // nvarchar(15)
		[Column,        Nullable] public string? Description  { get; set; } // ntext
		[Column,        Nullable] public byte[]? Picture      { get; set; } // image

		#region Associations

		/// <summary>
		/// FK_Products_Categories_BackReference (dbo.Products)
		/// </summary>
		[Association(ThisKey="CategoryID", OtherKey="CategoryID", CanBeNull=true)]
		public IEnumerable<Product> Products { get; set; } = null!;

		#endregion
	}

	[Table(Schema="dbo", Name="Category Sales for 1997", IsView=true)]
	public partial class CategorySalesFor1997
	{
		[Column, NotNull    ] public string   CategoryName  { get; set; } = null!; // nvarchar(15)
		[Column,    Nullable] public decimal? CategorySales { get; set; } // money
	}

	[Table(Schema="dbo", Name="Current Product List", IsView=true)]
	public partial class CurrentProductList
	{
		[Identity         ] public int    ProductID   { get; set; } // int
		[Column,   NotNull] public string ProductName { get; set; } = null!; // nvarchar(40)
	}

	[Table(Schema="dbo", Name="Customers")]
	public partial class Customer
	{
		[PrimaryKey, NotNull    ] public string  CustomerID   { get; set; } = null!; // nchar(5)
		[Column,     NotNull    ] public string  CompanyName  { get; set; } = null!; // nvarchar(40)
		[Column,        Nullable] public string? ContactName  { get; set; } // nvarchar(30)
		[Column,        Nullable] public string? ContactTitle { get; set; } // nvarchar(30)
		[Column,        Nullable] public string? Address      { get; set; } // nvarchar(60)
		[Column,        Nullable] public string? City         { get; set; } // nvarchar(15)
		[Column,        Nullable] public string? Region       { get; set; } // nvarchar(15)
		[Column,        Nullable] public string? PostalCode   { get; set; } // nvarchar(10)
		[Column,        Nullable] public string? Country      { get; set; } // nvarchar(15)
		[Column,        Nullable] public string? Phone        { get; set; } // nvarchar(24)
		[Column,        Nullable] public string? Fax          { get; set; } // nvarchar(24)

		#region Associations

		/// <summary>
		/// FK_CustomerCustomerDemo_Customers_BackReference (dbo.CustomerCustomerDemo)
		/// </summary>
		[Association(ThisKey="CustomerID", OtherKey="CustomerID", CanBeNull=true)]
		public IEnumerable<CustomerCustomerDemo> CustomerCustomerDemoes { get; set; } = null!;

		/// <summary>
		/// FK_Orders_Customers_BackReference (dbo.Orders)
		/// </summary>
		[Association(ThisKey="CustomerID", OtherKey="CustomerID", CanBeNull=true)]
		public IEnumerable<Order> Orders { get; set; } = null!;

		#endregion
	}

	[Table(Schema="dbo", Name="Customer and Suppliers by City", IsView=true)]
	public partial class CustomerAndSuppliersByCity
	{
		[Column,    Nullable] public string? City         { get; set; } // nvarchar(15)
		[Column, NotNull    ] public string  CompanyName  { get; set; } = null!; // nvarchar(40)
		[Column,    Nullable] public string? ContactName  { get; set; } // nvarchar(30)
		[Column, NotNull    ] public string  Relationship { get; set; } = null!; // varchar(9)
	}

	[Table(Schema="dbo", Name="CustomerCustomerDemo")]
	public partial class CustomerCustomerDemo
	{
		[PrimaryKey(1), NotNull] public string CustomerID     { get; set; } = null!; // nchar(5)
		[PrimaryKey(2), NotNull] public string CustomerTypeID { get; set; } = null!; // nchar(10)

		#region Associations

		/// <summary>
		/// FK_CustomerCustomerDemo_Customers (dbo.Customers)
		/// </summary>
		[Association(ThisKey="CustomerID", OtherKey="CustomerID", CanBeNull=false)]
		public Customer Customer { get; set; } = null!;

		/// <summary>
		/// FK_CustomerCustomerDemo (dbo.CustomerDemographics)
		/// </summary>
		[Association(ThisKey="CustomerTypeID", OtherKey="CustomerTypeID", CanBeNull=false)]
		public CustomerDemographic CustomerType { get; set; } = null!;

		#endregion
	}

	[Table(Schema="dbo", Name="CustomerDemographics")]
	public partial class CustomerDemographic
	{
		[PrimaryKey, NotNull    ] public string  CustomerTypeID { get; set; } = null!; // nchar(10)
		[Column,        Nullable] public string? CustomerDesc   { get; set; } // ntext

		#region Associations

		/// <summary>
		/// FK_CustomerCustomerDemo_BackReference (dbo.CustomerCustomerDemo)
		/// </summary>
		[Association(ThisKey="CustomerTypeID", OtherKey="CustomerTypeID", CanBeNull=true)]
		public IEnumerable<CustomerCustomerDemo> CustomerCustomerDemoes { get; set; } = null!;

		#endregion
	}

	[Table(Schema="dbo", Name="Employees")]
	public partial class Employee
	{
		[PrimaryKey, Identity   ] public int       EmployeeID      { get; set; } // int
		[Column,     NotNull    ] public string    LastName        { get; set; } = null!; // nvarchar(20)
		[Column,     NotNull    ] public string    FirstName       { get; set; } = null!; // nvarchar(10)
		[Column,        Nullable] public string?   Title           { get; set; } // nvarchar(30)
		[Column,        Nullable] public string?   TitleOfCourtesy { get; set; } // nvarchar(25)
		[Column,        Nullable] public DateTime? BirthDate       { get; set; } // datetime
		[Column,        Nullable] public DateTime? HireDate        { get; set; } // datetime
		[Column,        Nullable] public string?   Address         { get; set; } // nvarchar(60)
		[Column,        Nullable] public string?   City            { get; set; } // nvarchar(15)
		[Column,        Nullable] public string?   Region          { get; set; } // nvarchar(15)
		[Column,        Nullable] public string?   PostalCode      { get; set; } // nvarchar(10)
		[Column,        Nullable] public string?   Country         { get; set; } // nvarchar(15)
		[Column,        Nullable] public string?   HomePhone       { get; set; } // nvarchar(24)
		[Column,        Nullable] public string?   Extension       { get; set; } // nvarchar(4)
		[Column,        Nullable] public byte[]?   Photo           { get; set; } // image
		[Column,        Nullable] public string?   Notes           { get; set; } // ntext
		[Column,        Nullable] public int?      ReportsTo       { get; set; } // int
		[Column,        Nullable] public string?   PhotoPath       { get; set; } // nvarchar(255)

		#region Associations

		/// <summary>
		/// FK_EmployeeTerritories_Employees_BackReference (dbo.EmployeeTerritories)
		/// </summary>
		[Association(ThisKey="EmployeeID", OtherKey="EmployeeID", CanBeNull=true)]
		public IEnumerable<EmployeeTerritory> EmployeeTerritories { get; set; } = null!;

		/// <summary>
		/// FK_Employees_Employees (dbo.Employees)
		/// </summary>
		[Association(ThisKey="ReportsTo", OtherKey="EmployeeID", CanBeNull=true)]
		public Employee? FkEmployeesEmployee { get; set; }

		/// <summary>
		/// FK_Employees_Employees_BackReference (dbo.Employees)
		/// </summary>
		[Association(ThisKey="EmployeeID", OtherKey="ReportsTo", CanBeNull=true)]
		public IEnumerable<Employee> FkEmployeesEmployeesBackReferences { get; set; } = null!;

		/// <summary>
		/// FK_Orders_Employees_BackReference (dbo.Orders)
		/// </summary>
		[Association(ThisKey="EmployeeID", OtherKey="EmployeeID", CanBeNull=true)]
		public IEnumerable<Order> Orders { get; set; } = null!;

		#endregion
	}

	[Table(Schema="dbo", Name="EmployeeTerritories")]
	public partial class EmployeeTerritory
	{
		[PrimaryKey(1), NotNull] public int    EmployeeID  { get; set; } // int
		[PrimaryKey(2), NotNull] public string TerritoryID { get; set; } = null!; // nvarchar(20)

		#region Associations

		/// <summary>
		/// FK_EmployeeTerritories_Employees (dbo.Employees)
		/// </summary>
		[Association(ThisKey="EmployeeID", OtherKey="EmployeeID", CanBeNull=false)]
		public Employee Employee { get; set; } = null!;

		/// <summary>
		/// FK_EmployeeTerritories_Territories (dbo.Territories)
		/// </summary>
		[Association(ThisKey="TerritoryID", OtherKey="TerritoryID", CanBeNull=false)]
		public Territory Territory { get; set; } = null!;

		#endregion
	}

	[Table(Schema="dbo", Name="Invoices", IsView=true)]
	public partial class Invoice
	{
		[Column,    Nullable] public string?   ShipName       { get; set; } // nvarchar(40)
		[Column,    Nullable] public string?   ShipAddress    { get; set; } // nvarchar(60)
		[Column,    Nullable] public string?   ShipCity       { get; set; } // nvarchar(15)
		[Column,    Nullable] public string?   ShipRegion     { get; set; } // nvarchar(15)
		[Column,    Nullable] public string?   ShipPostalCode { get; set; } // nvarchar(10)
		[Column,    Nullable] public string?   ShipCountry    { get; set; } // nvarchar(15)
		[Column,    Nullable] public string?   CustomerID     { get; set; } // nchar(5)
		[Column, NotNull    ] public string    CustomerName   { get; set; } = null!; // nvarchar(40)
		[Column,    Nullable] public string?   Address        { get; set; } // nvarchar(60)
		[Column,    Nullable] public string?   City           { get; set; } // nvarchar(15)
		[Column,    Nullable] public string?   Region         { get; set; } // nvarchar(15)
		[Column,    Nullable] public string?   PostalCode     { get; set; } // nvarchar(10)
		[Column,    Nullable] public string?   Country        { get; set; } // nvarchar(15)
		[Column, NotNull    ] public string    Salesperson    { get; set; } = null!; // nvarchar(31)
		[Column, NotNull    ] public int       OrderID        { get; set; } // int
		[Column,    Nullable] public DateTime? OrderDate      { get; set; } // datetime
		[Column,    Nullable] public DateTime? RequiredDate   { get; set; } // datetime
		[Column,    Nullable] public DateTime? ShippedDate    { get; set; } // datetime
		[Column, NotNull    ] public string    ShipperName    { get; set; } = null!; // nvarchar(40)
		[Column, NotNull    ] public int       ProductID      { get; set; } // int
		[Column, NotNull    ] public string    ProductName    { get; set; } = null!; // nvarchar(40)
		[Column, NotNull    ] public decimal   UnitPrice      { get; set; } // money
		[Column, NotNull    ] public short     Quantity       { get; set; } // smallint
		[Column, NotNull    ] public float     Discount       { get; set; } // real
		[Column,    Nullable] public decimal?  ExtendedPrice  { get; set; } // money
		[Column,    Nullable] public decimal?  Freight        { get; set; } // money
	}

	[Table(Schema="dbo", Name="Orders")]
	public partial class Order
	{
		[PrimaryKey, Identity] public int       OrderID        { get; set; } // int
		[Column,     Nullable] public string?   CustomerID     { get; set; } // nchar(5)
		[Column,     Nullable] public int?      EmployeeID     { get; set; } // int
		[Column,     Nullable] public DateTime? OrderDate      { get; set; } // datetime
		[Column,     Nullable] public DateTime? RequiredDate   { get; set; } // datetime
		[Column,     Nullable] public DateTime? ShippedDate    { get; set; } // datetime
		[Column,     Nullable] public int?      ShipVia        { get; set; } // int
		[Column,     Nullable] public decimal?  Freight        { get; set; } // money
		[Column,     Nullable] public string?   ShipName       { get; set; } // nvarchar(40)
		[Column,     Nullable] public string?   ShipAddress    { get; set; } // nvarchar(60)
		[Column,     Nullable] public string?   ShipCity       { get; set; } // nvarchar(15)
		[Column,     Nullable] public string?   ShipRegion     { get; set; } // nvarchar(15)
		[Column,     Nullable] public string?   ShipPostalCode { get; set; } // nvarchar(10)
		[Column,     Nullable] public string?   ShipCountry    { get; set; } // nvarchar(15)

		#region Associations

		/// <summary>
		/// FK_Orders_Customers (dbo.Customers)
		/// </summary>
		[Association(ThisKey="CustomerID", OtherKey="CustomerID", CanBeNull=true)]
		public Customer? Customer { get; set; }

		/// <summary>
		/// FK_Orders_Employees (dbo.Employees)
		/// </summary>
		[Association(ThisKey="EmployeeID", OtherKey="EmployeeID", CanBeNull=true)]
		public Employee? Employee { get; set; }

		/// <summary>
		/// FK_Order_Details_Orders_BackReference (dbo.Order Details)
		/// </summary>
		[Association(ThisKey="OrderID", OtherKey="OrderID", CanBeNull=true)]
		public IEnumerable<OrderDetail> OrderDetails { get; set; } = null!;

		/// <summary>
		/// FK_Orders_Shippers (dbo.Shippers)
		/// </summary>
		[Association(ThisKey="ShipVia", OtherKey="ShipperID", CanBeNull=true)]
		public Shipper? Shipper { get; set; }

		#endregion
	}

	[Table(Schema="dbo", Name="Order Details")]
	public partial class OrderDetail
	{
		[PrimaryKey(1), NotNull] public int     OrderID   { get; set; } // int
		[PrimaryKey(2), NotNull] public int     ProductID { get; set; } // int
		[Column,        NotNull] public decimal UnitPrice { get; set; } // money
		[Column,        NotNull] public short   Quantity  { get; set; } // smallint
		[Column,        NotNull] public float   Discount  { get; set; } // real

		#region Associations

		/// <summary>
		/// FK_Order_Details_Orders (dbo.Orders)
		/// </summary>
		[Association(ThisKey="OrderID", OtherKey="OrderID", CanBeNull=false)]
		public Order Order { get; set; } = null!;

		/// <summary>
		/// FK_Order_Details_Products (dbo.Products)
		/// </summary>
		[Association(ThisKey="ProductID", OtherKey="ProductID", CanBeNull=false)]
		public Product Product { get; set; } = null!;

		#endregion
	}

	[Table(Schema="dbo", Name="Order Details Extended", IsView=true)]
	public partial class OrderDetailsExtended
	{
		[Column, NotNull    ] public int      OrderID       { get; set; } // int
		[Column, NotNull    ] public int      ProductID     { get; set; } // int
		[Column, NotNull    ] public string   ProductName   { get; set; } = null!; // nvarchar(40)
		[Column, NotNull    ] public decimal  UnitPrice     { get; set; } // money
		[Column, NotNull    ] public short    Quantity      { get; set; } // smallint
		[Column, NotNull    ] public float    Discount      { get; set; } // real
		[Column,    Nullable] public decimal? ExtendedPrice { get; set; } // money
	}

	[Table(Schema="dbo", Name="Orders Qry", IsView=true)]
	public partial class OrdersQry
	{
		[Column, NotNull    ] public int       OrderID        { get; set; } // int
		[Column,    Nullable] public string?   CustomerID     { get; set; } // nchar(5)
		[Column,    Nullable] public int?      EmployeeID     { get; set; } // int
		[Column,    Nullable] public DateTime? OrderDate      { get; set; } // datetime
		[Column,    Nullable] public DateTime? RequiredDate   { get; set; } // datetime
		[Column,    Nullable] public DateTime? ShippedDate    { get; set; } // datetime
		[Column,    Nullable] public int?      ShipVia        { get; set; } // int
		[Column,    Nullable] public decimal?  Freight        { get; set; } // money
		[Column,    Nullable] public string?   ShipName       { get; set; } // nvarchar(40)
		[Column,    Nullable] public string?   ShipAddress    { get; set; } // nvarchar(60)
		[Column,    Nullable] public string?   ShipCity       { get; set; } // nvarchar(15)
		[Column,    Nullable] public string?   ShipRegion     { get; set; } // nvarchar(15)
		[Column,    Nullable] public string?   ShipPostalCode { get; set; } // nvarchar(10)
		[Column,    Nullable] public string?   ShipCountry    { get; set; } // nvarchar(15)
		[Column, NotNull    ] public string    CompanyName    { get; set; } = null!; // nvarchar(40)
		[Column,    Nullable] public string?   Address        { get; set; } // nvarchar(60)
		[Column,    Nullable] public string?   City           { get; set; } // nvarchar(15)
		[Column,    Nullable] public string?   Region         { get; set; } // nvarchar(15)
		[Column,    Nullable] public string?   PostalCode     { get; set; } // nvarchar(10)
		[Column,    Nullable] public string?   Country        { get; set; } // nvarchar(15)
	}

	[Table(Schema="dbo", Name="Order Subtotals", IsView=true)]
	public partial class OrderSubtotal
	{
		[Column, NotNull    ] public int      OrderID  { get; set; } // int
		[Column,    Nullable] public decimal? Subtotal { get; set; } // money
	}

	[Table(Schema="dbo", Name="Products")]
	public partial class Product
	{
		[PrimaryKey, Identity   ] public int      ProductID       { get; set; } // int
		[Column,     NotNull    ] public string   ProductName     { get; set; } = null!; // nvarchar(40)
		[Column,        Nullable] public int?     SupplierID      { get; set; } // int
		[Column,        Nullable] public int?     CategoryID      { get; set; } // int
		[Column,        Nullable] public string?  QuantityPerUnit { get; set; } // nvarchar(20)
		[Column,        Nullable] public decimal? UnitPrice       { get; set; } // money
		[Column,        Nullable] public short?   UnitsInStock    { get; set; } // smallint
		[Column,        Nullable] public short?   UnitsOnOrder    { get; set; } // smallint
		[Column,        Nullable] public short?   ReorderLevel    { get; set; } // smallint
		[Column,     NotNull    ] public bool     Discontinued    { get; set; } // bit

		#region Associations

		/// <summary>
		/// FK_Products_Categories (dbo.Categories)
		/// </summary>
		[Association(ThisKey="CategoryID", OtherKey="CategoryID", CanBeNull=true)]
		public Category? Category { get; set; }

		/// <summary>
		/// FK_Order_Details_Products_BackReference (dbo.Order Details)
		/// </summary>
		[Association(ThisKey="ProductID", OtherKey="ProductID", CanBeNull=true)]
		public IEnumerable<OrderDetail> OrderDetails { get; set; } = null!;

		/// <summary>
		/// FK_Products_Suppliers (dbo.Suppliers)
		/// </summary>
		[Association(ThisKey="SupplierID", OtherKey="SupplierID", CanBeNull=true)]
		public Supplier? Supplier { get; set; }

		#endregion
	}

	[Table(Schema="dbo", Name="Products Above Average Price", IsView=true)]
	public partial class ProductsAboveAveragePrice
	{
		[Column, NotNull    ] public string   ProductName { get; set; } = null!; // nvarchar(40)
		[Column,    Nullable] public decimal? UnitPrice   { get; set; } // money
	}

	[Table(Schema="dbo", Name="Product Sales for 1997", IsView=true)]
	public partial class ProductSalesFor1997
	{
		[Column, NotNull    ] public string   CategoryName { get; set; } = null!; // nvarchar(15)
		[Column, NotNull    ] public string   ProductName  { get; set; } = null!; // nvarchar(40)
		[Column,    Nullable] public decimal? ProductSales { get; set; } // money
	}

	[Table(Schema="dbo", Name="Products by Category", IsView=true)]
	public partial class ProductsByCategory
	{
		[Column, NotNull    ] public string  CategoryName    { get; set; } = null!; // nvarchar(15)
		[Column, NotNull    ] public string  ProductName     { get; set; } = null!; // nvarchar(40)
		[Column,    Nullable] public string? QuantityPerUnit { get; set; } // nvarchar(20)
		[Column,    Nullable] public short?  UnitsInStock    { get; set; } // smallint
		[Column, NotNull    ] public bool    Discontinued    { get; set; } // bit
	}

	[Table(Schema="dbo", Name="Quarterly Orders", IsView=true)]
	public partial class QuarterlyOrder
	{
		[Column, Nullable] public string? CustomerID  { get; set; } // nchar(5)
		[Column, Nullable] public string? CompanyName { get; set; } // nvarchar(40)
		[Column, Nullable] public string? City        { get; set; } // nvarchar(15)
		[Column, Nullable] public string? Country     { get; set; } // nvarchar(15)
	}

	[Table(Schema="dbo", Name="Region")]
	public partial class Region
	{
		[PrimaryKey, NotNull] public int    RegionID          { get; set; } // int
		[Column,     NotNull] public string RegionDescription { get; set; } = null!; // nchar(50)

		#region Associations

		/// <summary>
		/// FK_Territories_Region_BackReference (dbo.Territories)
		/// </summary>
		[Association(ThisKey="RegionID", OtherKey="RegionID", CanBeNull=true)]
		public IEnumerable<Territory> Territories { get; set; } = null!;

		#endregion
	}

	[Table(Schema="dbo", Name="Sales by Category", IsView=true)]
	public partial class SalesByCategory
	{
		[Column, NotNull    ] public int      CategoryID   { get; set; } // int
		[Column, NotNull    ] public string   CategoryName { get; set; } = null!; // nvarchar(15)
		[Column, NotNull    ] public string   ProductName  { get; set; } = null!; // nvarchar(40)
		[Column,    Nullable] public decimal? ProductSales { get; set; } // money
	}

	[Table(Schema="dbo", Name="Sales Totals by Amount", IsView=true)]
	public partial class SalesTotalsByAmount
	{
		[Column,    Nullable] public decimal?  SaleAmount  { get; set; } // money
		[Column, NotNull    ] public int       OrderID     { get; set; } // int
		[Column, NotNull    ] public string    CompanyName { get; set; } = null!; // nvarchar(40)
		[Column,    Nullable] public DateTime? ShippedDate { get; set; } // datetime
	}

	[Table(Schema="dbo", Name="Shippers")]
	public partial class Shipper
	{
		[PrimaryKey, Identity   ] public int     ShipperID   { get; set; } // int
		[Column,     NotNull    ] public string  CompanyName { get; set; } = null!; // nvarchar(40)
		[Column,        Nullable] public string? Phone       { get; set; } // nvarchar(24)

		#region Associations

		/// <summary>
		/// FK_Orders_Shippers_BackReference (dbo.Orders)
		/// </summary>
		[Association(ThisKey="ShipperID", OtherKey="ShipVia", CanBeNull=true)]
		public IEnumerable<Order> Orders { get; set; } = null!;

		#endregion
	}

	[Table(Schema="dbo", Name="Summary of Sales by Quarter", IsView=true)]
	public partial class SummaryOfSalesByQuarter
	{
		[Column,    Nullable] public DateTime? ShippedDate { get; set; } // datetime
		[Column, NotNull    ] public int       OrderID     { get; set; } // int
		[Column,    Nullable] public decimal?  Subtotal    { get; set; } // money
	}

	[Table(Schema="dbo", Name="Summary of Sales by Year", IsView=true)]
	public partial class SummaryOfSalesByYear
	{
		[Column,    Nullable] public DateTime? ShippedDate { get; set; } // datetime
		[Column, NotNull    ] public int       OrderID     { get; set; } // int
		[Column,    Nullable] public decimal?  Subtotal    { get; set; } // money
	}

	[Table(Schema="dbo", Name="Suppliers")]
	public partial class Supplier
	{
		[PrimaryKey, Identity   ] public int     SupplierID   { get; set; } // int
		[Column,     NotNull    ] public string  CompanyName  { get; set; } = null!; // nvarchar(40)
		[Column,        Nullable] public string? ContactName  { get; set; } // nvarchar(30)
		[Column,        Nullable] public string? ContactTitle { get; set; } // nvarchar(30)
		[Column,        Nullable] public string? Address      { get; set; } // nvarchar(60)
		[Column,        Nullable] public string? City         { get; set; } // nvarchar(15)
		[Column,        Nullable] public string? Region       { get; set; } // nvarchar(15)
		[Column,        Nullable] public string? PostalCode   { get; set; } // nvarchar(10)
		[Column,        Nullable] public string? Country      { get; set; } // nvarchar(15)
		[Column,        Nullable] public string? Phone        { get; set; } // nvarchar(24)
		[Column,        Nullable] public string? Fax          { get; set; } // nvarchar(24)
		[Column,        Nullable] public string? HomePage     { get; set; } // ntext

		#region Associations

		/// <summary>
		/// FK_Products_Suppliers_BackReference (dbo.Products)
		/// </summary>
		[Association(ThisKey="SupplierID", OtherKey="SupplierID", CanBeNull=true)]
		public IEnumerable<Product> Products { get; set; } = null!;

		#endregion
	}

	[Table(Schema="dbo", Name="Territories")]
	public partial class Territory
	{
		[PrimaryKey, NotNull] public string TerritoryID          { get; set; } = null!; // nvarchar(20)
		[Column,     NotNull] public string TerritoryDescription { get; set; } = null!; // nchar(50)
		[Column,     NotNull] public int    RegionID             { get; set; } // int

		#region Associations

		/// <summary>
		/// FK_EmployeeTerritories_Territories_BackReference (dbo.EmployeeTerritories)
		/// </summary>
		[Association(ThisKey="TerritoryID", OtherKey="TerritoryID", CanBeNull=true)]
		public IEnumerable<EmployeeTerritory> EmployeeTerritories { get; set; } = null!;

		/// <summary>
		/// FK_Territories_Region (dbo.Region)
		/// </summary>
		[Association(ThisKey="RegionID", OtherKey="RegionID", CanBeNull=false)]
		public Region Region { get; set; } = null!;

		#endregion
	}

	public static partial class TestDataDBStoredProcedures
	{
		#region CustOrderHist

		public static IEnumerable<CustOrderHistResult> CustOrderHist(this TestDataDB dataConnection, string? @CustomerID)
		{
			var parameters = new []
			{
				new DataParameter("@CustomerID", @CustomerID, DBLayer.DataType.NChar)
				{
					Size = 5
				}
			};

			return dataConnection.QueryProc<CustOrderHistResult>("[dbo].[CustOrderHist]", parameters);
		}

		public partial class CustOrderHistResult
		{
			public string ProductName { get; set; } = null!;
			public int?   Total       { get; set; }
		}

		#endregion

		#region CustOrdersDetail

		public static IEnumerable<CustOrdersDetailResult> CustOrdersDetail(this TestDataDB dataConnection, int? @OrderID)
		{
			var parameters = new []
			{
				new DataParameter("@OrderID", @OrderID, DBLayer.DataType.Int32)
			};

			return dataConnection.QueryProc<CustOrdersDetailResult>("[dbo].[CustOrdersDetail]", parameters);
		}

		public partial class CustOrdersDetailResult
		{
			public string   ProductName   { get; set; } = null!;
			public decimal  UnitPrice     { get; set; }
			public short    Quantity      { get; set; }
			public int?     Discount      { get; set; }
			public decimal? ExtendedPrice { get; set; }
		}

		#endregion

		#region CustOrdersOrders

		public static IEnumerable<CustOrdersOrdersResult> CustOrdersOrders(this TestDataDB dataConnection, string? @CustomerID)
		{
			var parameters = new []
			{
				new DataParameter("@CustomerID", @CustomerID, DBLayer.DataType.NChar)
				{
					Size = 5
				}
			};

			return dataConnection.QueryProc<CustOrdersOrdersResult>("[dbo].[CustOrdersOrders]", parameters);
		}

		public partial class CustOrdersOrdersResult
		{
			public int       OrderID      { get; set; }
			public DateTime? OrderDate    { get; set; }
			public DateTime? RequiredDate { get; set; }
			public DateTime? ShippedDate  { get; set; }
		}

		#endregion

		#region EmployeeSalesByCountry

		public static IEnumerable<EmployeeSalesByCountryResult> EmployeeSalesByCountry(this TestDataDB dataConnection, DateTime? @Beginning_Date, DateTime? @Ending_Date)
		{
			var parameters = new []
			{
				new DataParameter("@Beginning_Date", @Beginning_Date, DBLayer.DataType.DateTime),
				new DataParameter("@Ending_Date",    @Ending_Date,    DBLayer.DataType.DateTime)
			};

			return dataConnection.QueryProc<EmployeeSalesByCountryResult>("[dbo].[Employee Sales by Country]", parameters);
		}

		public partial class EmployeeSalesByCountryResult
		{
			public string?   Country     { get; set; }
			public string    LastName    { get; set; } = null!;
			public string    FirstName   { get; set; } = null!;
			public DateTime? ShippedDate { get; set; }
			public int       OrderID     { get; set; }
			public decimal?  SaleAmount  { get; set; }
		}

		#endregion

		#region SalesByYear

		public static IEnumerable<SalesByYearResult> SalesByYear(this TestDataDB dataConnection, DateTime? @Beginning_Date, DateTime? @Ending_Date)
		{
			var parameters = new []
			{
				new DataParameter("@Beginning_Date", @Beginning_Date, DBLayer.DataType.DateTime),
				new DataParameter("@Ending_Date",    @Ending_Date,    DBLayer.DataType.DateTime)
			};

			return dataConnection.QueryProc<SalesByYearResult>("[dbo].[Sales by Year]", parameters);
		}

		public partial class SalesByYearResult
		{
			public DateTime? ShippedDate { get; set; }
			public int       OrderID     { get; set; }
			public decimal?  Subtotal    { get; set; }
			public string?   Year        { get; set; }
		}

		#endregion

		#region SalesByCategory

		public static IEnumerable<SalesByCategoryResult> SalesByCategory(this TestDataDB dataConnection, string? @CategoryName, string? @OrdYear)
		{
			var parameters = new []
			{
				new DataParameter("@CategoryName", @CategoryName, DBLayer.DataType.NVarChar)
				{
					Size = 15
				},
				new DataParameter("@OrdYear",      @OrdYear,      DBLayer.DataType.NVarChar)
				{
					Size = 4
				}
			};

			return dataConnection.QueryProc<SalesByCategoryResult>("[dbo].[SalesByCategory]", parameters);
		}

		public partial class SalesByCategoryResult
		{
			public string   ProductName   { get; set; } = null!;
			public decimal? TotalPurchase { get; set; }
		}

		#endregion

		#region TenMostExpensiveProducts

		public static IEnumerable<TenMostExpensiveProductsResult> TenMostExpensiveProducts(this TestDataDB dataConnection)
		{
			return dataConnection.QueryProc<TenMostExpensiveProductsResult>("[dbo].[Ten Most Expensive Products]");
		}

		public partial class TenMostExpensiveProductsResult
		{
			public string   TenMostExpensiveProducts { get; set; } = null!;
			public decimal? UnitPrice                { get; set; }
		}

		#endregion
	}

	public static partial class TableExtensions
	{
		public static Category? Find(this ITable<Category> table, int CategoryID)
		{
			return table.FirstOrDefault(t =>
				t.CategoryID == CategoryID);
		}

		public static Customer? Find(this ITable<Customer> table, string CustomerID)
		{
			return table.FirstOrDefault(t =>
				t.CustomerID == CustomerID);
		}

		public static CustomerCustomerDemo? Find(this ITable<CustomerCustomerDemo> table, string CustomerID, string CustomerTypeID)
		{
			return table.FirstOrDefault(t =>
				t.CustomerID     == CustomerID &&
				t.CustomerTypeID == CustomerTypeID);
		}

		public static CustomerDemographic? Find(this ITable<CustomerDemographic> table, string CustomerTypeID)
		{
			return table.FirstOrDefault(t =>
				t.CustomerTypeID == CustomerTypeID);
		}

		public static Employee? Find(this ITable<Employee> table, int EmployeeID)
		{
			return table.FirstOrDefault(t =>
				t.EmployeeID == EmployeeID);
		}

		public static EmployeeTerritory? Find(this ITable<EmployeeTerritory> table, int EmployeeID, string TerritoryID)
		{
			return table.FirstOrDefault(t =>
				t.EmployeeID  == EmployeeID &&
				t.TerritoryID == TerritoryID);
		}

		public static Order? Find(this ITable<Order> table, int OrderID)
		{
			return table.FirstOrDefault(t =>
				t.OrderID == OrderID);
		}

		public static OrderDetail? Find(this ITable<OrderDetail> table, int OrderID, int ProductID)
		{
			return table.FirstOrDefault(t =>
				t.OrderID   == OrderID &&
				t.ProductID == ProductID);
		}

		public static Product? Find(this ITable<Product> table, int ProductID)
		{
			return table.FirstOrDefault(t =>
				t.ProductID == ProductID);
		}

		public static Region? Find(this ITable<Region> table, int RegionID)
		{
			return table.FirstOrDefault(t =>
				t.RegionID == RegionID);
		}

		public static Shipper? Find(this ITable<Shipper> table, int ShipperID)
		{
			return table.FirstOrDefault(t =>
				t.ShipperID == ShipperID);
		}

		public static Supplier? Find(this ITable<Supplier> table, int SupplierID)
		{
			return table.FirstOrDefault(t =>
				t.SupplierID == SupplierID);
		}

		public static Territory? Find(this ITable<Territory> table, string TerritoryID)
		{
			return table.FirstOrDefault(t =>
				t.TerritoryID == TerritoryID);
		}
	}
}
