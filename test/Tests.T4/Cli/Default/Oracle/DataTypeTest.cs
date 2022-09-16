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
	[Table("DataTypeTest")]
	public class DataTypeTest
	{
		[Column("DataTypeID", IsPrimaryKey = true)] public decimal   DataTypeId { get; set; } // NUMBER
		[Column("Binary_"                        )] public byte[]?   Binary     { get; set; } // RAW(50)
		[Column("Boolean_"                       )] public sbyte?    Boolean    { get; set; } // NUMBER (1,0)
		[Column("Byte_"                          )] public short?    Byte       { get; set; } // NUMBER (3,0)
		[Column("Bytes_"                         )] public byte[]?   Bytes      { get; set; } // BLOB
		[Column("Char_"                          )] public char?     Char       { get; set; } // NCHAR(1)
		[Column("DateTime_"                      )] public DateTime? DateTime   { get; set; } // DATE
		[Column("Decimal_"                       )] public decimal?  Decimal    { get; set; } // NUMBER (19,5)
		[Column("Double_"                        )] public decimal?  Double     { get; set; } // FLOAT(126)
		[Column("Guid_"                          )] public byte[]?   Guid       { get; set; } // RAW(16)
		[Column("Int16_"                         )] public int?      Int16      { get; set; } // NUMBER (5,0)
		[Column("Int32_"                         )] public long?     Int32      { get; set; } // NUMBER (10,0)
		[Column("Int64_"                         )] public decimal?  Int64      { get; set; } // NUMBER (20,0)
		[Column("Money_"                         )] public decimal?  Money      { get; set; } // NUMBER
		[Column("SByte_"                         )] public short?    SByte      { get; set; } // NUMBER (3,0)
		[Column("Single_"                        )] public decimal?  Single     { get; set; } // FLOAT(126)
		[Column("Stream_"                        )] public byte[]?   Stream     { get; set; } // BLOB
		[Column("String_"                        )] public string?   String     { get; set; } // NVARCHAR2(50)
		[Column("UInt16_"                        )] public int?      UInt16     { get; set; } // NUMBER (5,0)
		[Column("UInt32_"                        )] public long?     UInt32     { get; set; } // NUMBER (10,0)
		[Column("UInt64_"                        )] public decimal?  UInt64     { get; set; } // NUMBER (20,0)
		[Column("Xml_"                           )] public string?   Xml        { get; set; } // XMLTYPE
	}
}
