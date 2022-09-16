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

namespace Cli.Default.Access.OleDb
{
	[Table("DataTypeTest")]
	public class DataTypeTest
	{
		[Column("DataTypeID", IsPrimaryKey = true)] public int       DataTypeId { get; set; } // Long
		[Column("Binary_"                        )] public byte[]?   Binary     { get; set; } // LongBinary
		[Column("Boolean_"                       )] public int?      Boolean    { get; set; } // Long
		[Column("Byte_"                          )] public byte?     Byte       { get; set; } // Byte
		[Column("Bytes_"                         )] public byte[]?   Bytes      { get; set; } // LongBinary
		[Column("Char_"                          )] public char?     Char       { get; set; } // VarChar(1)
		[Column("DateTime_"                      )] public DateTime? DateTime   { get; set; } // DateTime
		[Column("Decimal_"                       )] public decimal?  Decimal    { get; set; } // Currency
		[Column("Double_"                        )] public double?   Double     { get; set; } // Double
		[Column("Guid_"                          )] public Guid?     Guid       { get; set; } // GUID
		[Column("Int16_"                         )] public short?    Int16      { get; set; } // Short
		[Column("Int32_"                         )] public int?      Int32      { get; set; } // Long
		[Column("Int64_"                         )] public int?      Int64      { get; set; } // Long
		[Column("Money_"                         )] public decimal?  Money      { get; set; } // Currency
		[Column("SByte_"                         )] public byte?     SByte      { get; set; } // Byte
		[Column("Single_"                        )] public float?    Single     { get; set; } // Single
		[Column("Stream_"                        )] public byte[]?   Stream     { get; set; } // LongBinary
		[Column("String_"                        )] public string?   String     { get; set; } // VarChar(50)
		[Column("UInt16_"                        )] public short?    UInt16     { get; set; } // Short
		[Column("UInt32_"                        )] public int?      UInt32     { get; set; } // Long
		[Column("UInt64_"                        )] public int?      UInt64     { get; set; } // Long
		[Column("Xml_"                           )] public string?   Xml        { get; set; } // LongText
	}
}
