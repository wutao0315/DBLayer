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
	[Table("EmployeeTerritories")]
	public class EmployeeTerritory
	{
		[Column("EmployeeID" , IsPrimaryKey = true , PrimaryKeyOrder = 0                        )] public int    EmployeeId  { get; set; } // int
		[Column("TerritoryID", CanBeNull    = false, IsPrimaryKey    = true, PrimaryKeyOrder = 1)] public string TerritoryId { get; set; } = null!; // nvarchar(20)

		#region Associations
		/// <summary>
		/// FK_EmployeeTerritories_Employees
		/// </summary>
		[Association(CanBeNull = false, ThisKey = nameof(EmployeeId), OtherKey = nameof(SqlServerNorthwind.Employee.EmployeeId))]
		public Employee Employee { get; set; } = null!;

		/// <summary>
		/// FK_EmployeeTerritories_Territories
		/// </summary>
		[Association(CanBeNull = false, ThisKey = nameof(TerritoryId), OtherKey = nameof(SqlServerNorthwind.Territory.TerritoryId))]
		public Territory Territory { get; set; } = null!;
		#endregion
	}
}
