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
	[Table("AllTypes")]
	public class AllType
	{
		[Column("ID"                    , IsPrimaryKey = true)] public decimal         Id                     { get; set; } // NUMBER
		[Column("bigintDataType"                             )] public decimal?        BigintDataType         { get; set; } // NUMBER (20,0)
		[Column("numericDataType"                            )] public decimal?        NumericDataType        { get; set; } // NUMBER
		[Column("bitDataType"                                )] public sbyte?          BitDataType            { get; set; } // NUMBER (1,0)
		[Column("smallintDataType"                           )] public int?            SmallintDataType       { get; set; } // NUMBER (5,0)
		[Column("decimalDataType"                            )] public decimal?        DecimalDataType        { get; set; } // NUMBER
		[Column("smallmoneyDataType"                         )] public decimal?        SmallmoneyDataType     { get; set; } // NUMBER (10,4)
		[Column("intDataType"                                )] public long?           IntDataType            { get; set; } // NUMBER (10,0)
		[Column("tinyintDataType"                            )] public short?          TinyintDataType        { get; set; } // NUMBER (3,0)
		[Column("moneyDataType"                              )] public decimal?        MoneyDataType          { get; set; } // NUMBER
		[Column("floatDataType"                              )] public double?         FloatDataType          { get; set; } // BINARY_DOUBLE
		[Column("realDataType"                               )] public float?          RealDataType           { get; set; } // BINARY_FLOAT
		[Column("datetimeDataType"                           )] public DateTime?       DatetimeDataType       { get; set; } // DATE
		[Column("datetime2DataType"                          )] public DateTime?       Datetime2DataType      { get; set; } // TIMESTAMP(6)
		[Column("datetimeoffsetDataType"                     )] public DateTimeOffset? DatetimeoffsetDataType { get; set; } // TIMESTAMP(6) WITH TIME ZONE
		[Column("localZoneDataType"                          )] public DateTimeOffset? LocalZoneDataType      { get; set; } // TIMESTAMP(6) WITH LOCAL TIME ZONE
		[Column("charDataType"                               )] public char?           CharDataType           { get; set; } // CHAR(1)
		[Column("char20DataType"                             )] public string?         Char20DataType         { get; set; } // CHAR(20)
		[Column("varcharDataType"                            )] public string?         VarcharDataType        { get; set; } // VARCHAR2(20)
		[Column("textDataType"                               )] public string?         TextDataType           { get; set; } // CLOB
		[Column("ncharDataType"                              )] public string?         NcharDataType          { get; set; } // NCHAR(20)
		[Column("nvarcharDataType"                           )] public string?         NvarcharDataType       { get; set; } // NVARCHAR2(20)
		[Column("ntextDataType"                              )] public string?         NtextDataType          { get; set; } // NCLOB
		[Column("binaryDataType"                             )] public byte[]?         BinaryDataType         { get; set; } // BLOB
		[Column("bfileDataType"                              )] public byte[]?         BfileDataType          { get; set; } // BFILE
		[Column("guidDataType"                               )] public byte[]?         GuidDataType           { get; set; } // RAW(16)
		[Column("longDataType"                               )] public string?         LongDataType           { get; set; } // LONG
		[Column("xmlDataType"                                )] public string?         XmlDataType            { get; set; } // XMLTYPE
	}
}