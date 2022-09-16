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

namespace Cli.Default.Oracle
{
	[Table("LINQDATATYPESBC")]
	public class Linqdatatypesbc
	{
		[Column("ID"            )] public decimal?  Id             { get; set; } // NUMBER
		[Column("MONEYVALUE"    )] public decimal?  Moneyvalue     { get; set; } // NUMBER (10,4)
		[Column("DATETIMEVALUE" )] public DateTime? Datetimevalue  { get; set; } // TIMESTAMP(6)
		[Column("DATETIMEVALUE2")] public DateTime? Datetimevalue2 { get; set; } // TIMESTAMP(6)
		[Column("BOOLVALUE"     )] public decimal?  Boolvalue      { get; set; } // NUMBER
		[Column("GUIDVALUE"     )] public byte[]?   Guidvalue      { get; set; } // RAW(16)
		[Column("SMALLINTVALUE" )] public decimal?  Smallintvalue  { get; set; } // NUMBER
		[Column("INTVALUE"      )] public decimal?  Intvalue       { get; set; } // NUMBER
		[Column("BIGINTVALUE"   )] public decimal?  Bigintvalue    { get; set; } // NUMBER (20,0)
		[Column("STRINGVALUE"   )] public string?   Stringvalue    { get; set; } // VARCHAR2(50)
	}
}