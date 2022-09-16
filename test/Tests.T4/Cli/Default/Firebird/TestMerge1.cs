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

namespace Cli.Default.Firebird
{
	[Table("TestMerge1")]
	public class TestMerge1
	{
		[Column("Id"             , IsPrimaryKey = true)] public int       Id              { get; set; } // integer
		[Column("Field1"                              )] public int?      Field1          { get; set; } // integer
		[Column("Field2"                              )] public int?      Field2          { get; set; } // integer
		[Column("Field3"                              )] public int?      Field3          { get; set; } // integer
		[Column("Field4"                              )] public int?      Field4          { get; set; } // integer
		[Column("Field5"                              )] public int?      Field5          { get; set; } // integer
		[Column("FieldInt64"                          )] public long?     FieldInt64      { get; set; } // bigint
		[Column("FieldBoolean"                        )] public char?     FieldBoolean    { get; set; } // char(1)
		[Column("FieldString"                         )] public string?   FieldString     { get; set; } // varchar(20)
		[Column("FieldNString"                        )] public string?   FieldNString    { get; set; } // varchar(20)
		[Column("FieldChar"                           )] public char?     FieldChar       { get; set; } // char(1)
		[Column("FieldNChar"                          )] public char?     FieldNChar      { get; set; } // char(1)
		[Column("FieldFloat"                          )] public float?    FieldFloat      { get; set; } // float
		[Column("FieldDouble"                         )] public double?   FieldDouble     { get; set; } // double precision
		[Column("FieldDateTime"                       )] public DateTime? FieldDateTime   { get; set; } // timestamp
		[Column("FieldBinary"                         )] public byte[]?   FieldBinary     { get; set; } // blob
		[Column("FieldGuid"                           )] public string?   FieldGuid       { get; set; } // char(16)
		[Column("FieldDecimal"                        )] public decimal?  FieldDecimal    { get; set; } // decimal(18,10)
		[Column("FieldDate"                           )] public DateTime? FieldDate       { get; set; } // date
		[Column("FieldTime"                           )] public DateTime? FieldTime       { get; set; } // timestamp
		[Column("FieldEnumString"                     )] public string?   FieldEnumString { get; set; } // varchar(20)
		[Column("FieldEnumNumber"                     )] public int?      FieldEnumNumber { get; set; } // integer
	}
}