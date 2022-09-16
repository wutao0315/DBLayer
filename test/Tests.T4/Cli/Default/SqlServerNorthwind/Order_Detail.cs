// ---------------------------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by DBLayer scaffolding tool (https://github.com/linq2db/linq2db).
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
// ---------------------------------------------------------------------------------------------------

using DBLayer.Mapping;

#pragma warning disable 1573, 1591
#nullable enable

namespace Cli.Default.SqlServerNorthwind
{
	[Table("Order Details")]
	public class OrderDetail
	{
		[Column("OrderID"  , IsPrimaryKey = true, PrimaryKeyOrder = 0)] public int     OrderId   { get; set; } // int
		[Column("ProductID", IsPrimaryKey = true, PrimaryKeyOrder = 1)] public int     ProductId { get; set; } // int
		[Column("UnitPrice"                                          )] public decimal UnitPrice { get; set; } // money
		[Column("Quantity"                                           )] public short   Quantity  { get; set; } // smallint
		[Column("Discount"                                           )] public float   Discount  { get; set; } // real

		#region Associations
		/// <summary>
		/// FK_Order_Details_Orders
		/// </summary>
		[Association(CanBeNull = false, ThisKey = nameof(OrderId), OtherKey = nameof(SqlServerNorthwind.Order.OrderId))]
		public Order Order { get; set; } = null!;

		/// <summary>
		/// FK_Order_Details_Products
		/// </summary>
		[Association(CanBeNull = false, ThisKey = nameof(ProductId), OtherKey = nameof(SqlServerNorthwind.Product.ProductId))]
		public Product Product { get; set; } = null!;
		#endregion
	}
}
