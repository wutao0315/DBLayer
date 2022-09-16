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

namespace Cli.Default.SqlServer
{
	[Table("LinqDataTypes")]
	public class LinqDataType
	{
		[Column("_ID"           , IsPrimaryKey = true, IsIdentity = true, SkipOnInsert = true, SkipOnUpdate = true)] public int       Id             { get; set; } // int
		[Column("ID"                                                                                              )] public int?      Id1            { get; set; } // int
		[Column("MoneyValue"                                                                                      )] public decimal?  MoneyValue     { get; set; } // decimal(10, 4)
		[Column("DateTimeValue"                                                                                   )] public DateTime? DateTimeValue  { get; set; } // datetime
		[Column("DateTimeValue2"                                                                                  )] public DateTime? DateTimeValue2 { get; set; } // datetime2(7)
		[Column("BoolValue"                                                                                       )] public bool?     BoolValue      { get; set; } // bit
		[Column("GuidValue"                                                                                       )] public Guid?     GuidValue      { get; set; } // uniqueidentifier
		[Column("BinaryValue"                                                                                     )] public byte[]?   BinaryValue    { get; set; } // varbinary(5000)
		[Column("SmallIntValue"                                                                                   )] public short?    SmallIntValue  { get; set; } // smallint
		[Column("IntValue"                                                                                        )] public int?      IntValue       { get; set; } // int
		[Column("BigIntValue"                                                                                     )] public long?     BigIntValue    { get; set; } // bigint
		[Column("StringValue"                                                                                     )] public string?   StringValue    { get; set; } // nvarchar(50)
	}
}
