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

namespace Cli.Default.SQLite
{
	[Table("TestMerge2")]
	public class TestMerge2
	{
		[Column("Id"             )] public long      Id              { get; set; } // integer
		[Column("Field1"         )] public long?     Field1          { get; set; } // integer
		[Column("Field2"         )] public long?     Field2          { get; set; } // integer
		[Column("Field3"         )] public long?     Field3          { get; set; } // integer
		[Column("Field4"         )] public long?     Field4          { get; set; } // integer
		[Column("Field5"         )] public long?     Field5          { get; set; } // integer
		[Column("FieldInt64"     )] public long?     FieldInt64      { get; set; } // bigint
		[Column("FieldBoolean"   )] public bool?     FieldBoolean    { get; set; } // bit
		[Column("FieldString"    )] public string?   FieldString     { get; set; } // varchar(20)
		[Column("FieldNString"   )] public string?   FieldNString    { get; set; } // nvarchar(20)
		[Column("FieldChar"      )] public char?     FieldChar       { get; set; } // char(1)
		[Column("FieldNChar"     )] public char?     FieldNChar      { get; set; } // char(1)
		[Column("FieldFloat"     )] public double?   FieldFloat      { get; set; } // float
		[Column("FieldDouble"    )] public double?   FieldDouble     { get; set; } // float
		[Column("FieldDateTime"  )] public DateTime? FieldDateTime   { get; set; } // datetime
		[Column("FieldBinary"    )] public byte[]?   FieldBinary     { get; set; } // varbinary
		[Column("FieldGuid"      )] public Guid?     FieldGuid       { get; set; } // uniqueidentifier
		[Column("FieldDate"      )] public DateTime? FieldDate       { get; set; } // date
		[Column("FieldEnumString")] public string?   FieldEnumString { get; set; } // varchar(20)
		[Column("FieldEnumNumber")] public int?      FieldEnumNumber { get; set; } // int
	}
}