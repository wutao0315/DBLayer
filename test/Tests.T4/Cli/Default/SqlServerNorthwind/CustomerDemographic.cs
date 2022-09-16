// ---------------------------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by DBLayer scaffolding tool (https://github.com/linq2db/linq2db).
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
// ---------------------------------------------------------------------------------------------------

using DBLayer.Mapping;
using System.Collections.Generic;

#pragma warning disable 1573, 1591
#nullable enable

namespace Cli.Default.SqlServerNorthwind
{
	[Table("CustomerDemographics")]
	public class CustomerDemographic
	{
		[Column("CustomerTypeID", CanBeNull = false, IsPrimaryKey = true)] public string  CustomerTypeId { get; set; } = null!; // nchar(10)
		[Column("CustomerDesc"                                          )] public string? CustomerDesc   { get; set; } // ntext

		#region Associations
		/// <summary>
		/// FK_CustomerCustomerDemo backreference
		/// </summary>
		[Association(ThisKey = nameof(CustomerTypeId), OtherKey = nameof(CustomerCustomerDemo.CustomerTypeId))]
		public IEnumerable<CustomerCustomerDemo> CustomerCustomerDemos { get; set; } = null!;
		#endregion
	}
}
