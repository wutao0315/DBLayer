// ---------------------------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by DBLayer scaffolding tool (https://github.com/linq2db/linq2db).
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
// ---------------------------------------------------------------------------------------------------

using DBLayer.Mapping;

#pragma warning disable 1573, 1591
#nullable enable

namespace Cli.Default.SQLiteNorthwind
{
	[Table("Products by Category", IsView = true)]
	public class ProductsByCategory
	{
		[Column("CategoryName"   , CanBeNull = false)] public string  CategoryName    { get; set; } = null!; // varchar(15)
		[Column("ProductName"    , CanBeNull = false)] public string  ProductName     { get; set; } = null!; // varchar(40)
		[Column("QuantityPerUnit"                   )] public string? QuantityPerUnit { get; set; } // varchar(20)
		[Column("UnitsInStock"                      )] public int?    UnitsInStock    { get; set; } // int
		[Column("Discontinued"                      )] public int     Discontinued    { get; set; } // int
	}
}
