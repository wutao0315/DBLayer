// ---------------------------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by DBLayer scaffolding tool (https://github.com/linq2db/linq2db).
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
// ---------------------------------------------------------------------------------------------------

using DBLayer;
using DBLayer.Configuration;
using DBLayer.Data;
using DBLayer.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable 1573, 1591
#nullable enable

namespace Cli.T4.Informix
{
	public partial class TestDataDB : DataConnection
	{
		public TestDataDB()
		{
			InitDataContext();
		}

		public TestDataDB(string configuration)
			: base(configuration)
		{
			InitDataContext();
		}

		public TestDataDB(DBLayerConnectionOptions options)
			: base(options)
		{
			InitDataContext();
		}

		public TestDataDB(DBLayerConnectionOptions<TestDataDB> options)
			: base(options)
		{
			InitDataContext();
		}

		partial void InitDataContext();

		public ITable<Inheritanceparent> Inheritanceparents  => this.GetTable<Inheritanceparent>();
		public ITable<Inheritancechild>  Inheritancechildren => this.GetTable<Inheritancechild>();
		public ITable<Person>            People              => this.GetTable<Person>();
		public ITable<Doctor>            Doctors             => this.GetTable<Doctor>();
		public ITable<Patient>           Patients            => this.GetTable<Patient>();
		public ITable<Parent>            Parents             => this.GetTable<Parent>();
		public ITable<Child>             Children            => this.GetTable<Child>();
		public ITable<Grandchild>        Grandchildren       => this.GetTable<Grandchild>();
		public ITable<Linqdatatype>      Linqdatatypes       => this.GetTable<Linqdatatype>();
		public ITable<Testidentity>      Testidentities      => this.GetTable<Testidentity>();
		public ITable<Alltype>           Alltypes            => this.GetTable<Alltype>();
		public ITable<Testunique>        Testuniques         => this.GetTable<Testunique>();
		public ITable<Testfkunique>      Testfkuniques       => this.GetTable<Testfkunique>();
		public ITable<Testmerge1>        Testmerge1          => this.GetTable<Testmerge1>();
		public ITable<Testmerge2>        Testmerge2          => this.GetTable<Testmerge2>();
		public ITable<Collatedtable>     Collatedtables      => this.GetTable<Collatedtable>();
		public ITable<Personview>        Personviews         => this.GetTable<Personview>();
	}

	[Table("inheritanceparent", Schema = "informix")]
	public partial class Inheritanceparent
	{
		[Column("inheritanceparentid", IsPrimaryKey = true)] public int     Inheritanceparentid { get; set; } // INTEGER
		[Column("typediscriminator"                       )] public int?    Typediscriminator   { get; set; } // INTEGER
		[Column("name"                                    )] public string? Name                { get; set; } // NVARCHAR(50)
	}

	public static partial class ExtensionMethods
	{
		#region Table Extensions
		public static Inheritanceparent? Find(this ITable<Inheritanceparent> table, int inheritanceparentid)
		{
			return table.FirstOrDefault(e => e.Inheritanceparentid == inheritanceparentid);
		}

		public static Inheritancechild? Find(this ITable<Inheritancechild> table, int inheritancechildid)
		{
			return table.FirstOrDefault(e => e.Inheritancechildid == inheritancechildid);
		}

		public static Person? Find(this ITable<Person> table, int personid)
		{
			return table.FirstOrDefault(e => e.Personid == personid);
		}

		public static Doctor? Find(this ITable<Doctor> table, int personid)
		{
			return table.FirstOrDefault(e => e.Personid == personid);
		}

		public static Patient? Find(this ITable<Patient> table, int personid)
		{
			return table.FirstOrDefault(e => e.Personid == personid);
		}

		public static Testidentity? Find(this ITable<Testidentity> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Alltype? Find(this ITable<Alltype> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Testunique? Find(this ITable<Testunique> table, int id1, int id2)
		{
			return table.FirstOrDefault(e => e.Id1 == id1 && e.Id2 == id2);
		}

		public static Testmerge1? Find(this ITable<Testmerge1> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Testmerge2? Find(this ITable<Testmerge2> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}
		#endregion
	}

	[Table("inheritancechild", Schema = "informix")]
	public partial class Inheritancechild
	{
		[Column("inheritancechildid" , IsPrimaryKey = true)] public int     Inheritancechildid  { get; set; } // INTEGER
		[Column("inheritanceparentid"                     )] public int     Inheritanceparentid { get; set; } // INTEGER
		[Column("typediscriminator"                       )] public int?    Typediscriminator   { get; set; } // INTEGER
		[Column("name"                                    )] public string? Name                { get; set; } // NVARCHAR(50)
	}

	[Table("person", Schema = "informix")]
	public partial class Person
	{
		[Column("personid"  , IsPrimaryKey = true , IsIdentity = true, SkipOnInsert = true, SkipOnUpdate = true)] public int     Personid   { get; set; } // SERIAL
		[Column("firstname" , CanBeNull    = false                                                             )] public string  Firstname  { get; set; } = null!; // NVARCHAR(50)
		[Column("lastname"  , CanBeNull    = false                                                             )] public string  Lastname   { get; set; } = null!; // NVARCHAR(50)
		[Column("middlename"                                                                                   )] public string? Middlename { get; set; } // NVARCHAR(50)
		[Column("gender"                                                                                       )] public char    Gender     { get; set; } // CHAR(1)

		#region Associations
		/// <summary>
		/// FK_doctor_person backreference
		/// </summary>
		[Association(ThisKey = nameof(Personid), OtherKey = nameof(Informix.Doctor.Personid))]
		public Doctor? Doctor { get; set; }

		/// <summary>
		/// FK_patient_person backreference
		/// </summary>
		[Association(ThisKey = nameof(Personid), OtherKey = nameof(Informix.Patient.Personid))]
		public Patient? Patient { get; set; }
		#endregion
	}

	[Table("doctor", Schema = "informix")]
	public partial class Doctor
	{
		[Column("personid", IsPrimaryKey = true )] public int    Personid { get; set; } // INTEGER
		[Column("taxonomy", CanBeNull    = false)] public string Taxonomy { get; set; } = null!; // NVARCHAR(50)

		#region Associations
		/// <summary>
		/// FK_doctor_person
		/// </summary>
		[Association(CanBeNull = false, ThisKey = nameof(Personid), OtherKey = nameof(Informix.Person.Personid))]
		public Person Person { get; set; } = null!;
		#endregion
	}

	[Table("patient", Schema = "informix")]
	public partial class Patient
	{
		[Column("personid" , IsPrimaryKey = true )] public int    Personid  { get; set; } // INTEGER
		[Column("diagnosis", CanBeNull    = false)] public string Diagnosis { get; set; } = null!; // NVARCHAR(100)

		#region Associations
		/// <summary>
		/// FK_patient_person
		/// </summary>
		[Association(CanBeNull = false, ThisKey = nameof(Personid), OtherKey = nameof(Informix.Person.Personid))]
		public Person Person { get; set; } = null!;
		#endregion
	}

	[Table("parent", Schema = "informix")]
	public partial class Parent
	{
		[Column("parentid")] public int? Parentid { get; set; } // INTEGER
		[Column("value1"  )] public int? Value1   { get; set; } // INTEGER
	}

	[Table("child", Schema = "informix")]
	public partial class Child
	{
		[Column("parentid")] public int? Parentid { get; set; } // INTEGER
		[Column("childid" )] public int? Childid  { get; set; } // INTEGER
	}

	[Table("grandchild", Schema = "informix")]
	public partial class Grandchild
	{
		[Column("parentid"    )] public int? Parentid     { get; set; } // INTEGER
		[Column("childid"     )] public int? Childid      { get; set; } // INTEGER
		[Column("grandchildid")] public int? Grandchildid { get; set; } // INTEGER
	}

	[Table("linqdatatypes", Schema = "informix")]
	public partial class Linqdatatype
	{
		[Column("id"            )] public int?      Id             { get; set; } // INTEGER
		[Column("moneyvalue"    )] public decimal?  Moneyvalue     { get; set; } // DECIMAL(10,4)
		[Column("datetimevalue" )] public DateTime? Datetimevalue  { get; set; } // DATETIME YEAR TO FRACTION(3)
		[Column("datetimevalue2")] public DateTime? Datetimevalue2 { get; set; } // DATETIME YEAR TO FRACTION(3)
		[Column("boolvalue"     )] public bool?     Boolvalue      { get; set; } // BOOLEAN
		[Column("guidvalue"     )] public string?   Guidvalue      { get; set; } // CHAR(36)
		[Column("binaryvalue"   )] public byte[]?   Binaryvalue    { get; set; } // BYTE
		[Column("smallintvalue" )] public short?    Smallintvalue  { get; set; } // SMALLINT
		[Column("intvalue"      )] public int?      Intvalue       { get; set; } // INTEGER
		[Column("bigintvalue"   )] public long?     Bigintvalue    { get; set; } // BIGINT
		[Column("stringvalue"   )] public string?   Stringvalue    { get; set; } // NVARCHAR(50)
	}

	[Table("testidentity", Schema = "informix")]
	public partial class Testidentity
	{
		[Column("id", IsPrimaryKey = true, IsIdentity = true, SkipOnInsert = true, SkipOnUpdate = true)] public int Id { get; set; } // SERIAL
	}

	[Table("alltypes", Schema = "informix")]
	public partial class Alltype
	{
		[Column("id"              , IsPrimaryKey = true, IsIdentity = true, SkipOnInsert = true, SkipOnUpdate = true)] public int       Id               { get; set; } // SERIAL
		[Column("bigintdatatype"                                                                                    )] public long?     Bigintdatatype   { get; set; } // BIGINT
		[Column("int8datatype"                                                                                      )] public long?     Int8Datatype     { get; set; } // INT8
		[Column("intdatatype"                                                                                       )] public int?      Intdatatype      { get; set; } // INTEGER
		[Column("smallintdatatype"                                                                                  )] public short?    Smallintdatatype { get; set; } // SMALLINT
		[Column("decimaldatatype"                                                                                   )] public decimal?  Decimaldatatype  { get; set; } // DECIMAL
		[Column("moneydatatype"                                                                                     )] public decimal?  Moneydatatype    { get; set; } // MONEY(16,2)
		[Column("realdatatype"                                                                                      )] public float?    Realdatatype     { get; set; } // SMALLFLOAT
		[Column("floatdatatype"                                                                                     )] public double?   Floatdatatype    { get; set; } // FLOAT
		[Column("booldatatype"                                                                                      )] public bool?     Booldatatype     { get; set; } // BOOLEAN
		[Column("chardatatype"                                                                                      )] public char?     Chardatatype     { get; set; } // CHAR(1)
		[Column("char20datatype"                                                                                    )] public string?   Char20Datatype   { get; set; } // CHAR(20)
		[Column("varchardatatype"                                                                                   )] public string?   Varchardatatype  { get; set; } // VARCHAR(10)
		[Column("nchardatatype"                                                                                     )] public string?   Nchardatatype    { get; set; } // NCHAR(10)
		[Column("nvarchardatatype"                                                                                  )] public string?   Nvarchardatatype { get; set; } // NVARCHAR(10)
		[Column("lvarchardatatype"                                                                                  )] public string?   Lvarchardatatype { get; set; } // LVARCHAR(10)
		[Column("textdatatype"                                                                                      )] public string?   Textdatatype     { get; set; } // TEXT
		[Column("datedatatype"                                                                                      )] public DateTime? Datedatatype     { get; set; } // DATE
		[Column("datetimedatatype"                                                                                  )] public DateTime? Datetimedatatype { get; set; } // DATETIME YEAR TO SECOND
		[Column("intervaldatatype"                                                                                  )] public TimeSpan? Intervaldatatype { get; set; } // INTERVAL HOUR TO SECOND
		[Column("bytedatatype"                                                                                      )] public byte[]?   Bytedatatype     { get; set; } // BYTE
	}

	[Table("testunique", Schema = "informix")]
	public partial class Testunique
	{
		[Column("id1", IsPrimaryKey = true, PrimaryKeyOrder = 0)] public int Id1 { get; set; } // INTEGER
		[Column("id2", IsPrimaryKey = true, PrimaryKeyOrder = 1)] public int Id2 { get; set; } // INTEGER
		[Column("id3"                                          )] public int Id3 { get; set; } // INTEGER
		[Column("id4"                                          )] public int Id4 { get; set; } // INTEGER

		#region Associations
		/// <summary>
		/// FK_testfkunique_testunique backreference
		/// </summary>
		[Association(ThisKey = nameof(Id1) + "," + nameof(Id1), OtherKey = nameof(Testfkunique.Id1) + "," + nameof(Id1))]
		public IEnumerable<Testfkunique> Testfkuniques { get; set; } = null!;

		/// <summary>
		/// FK_testfkunique_testunique_1 backreference
		/// </summary>
		[Association(ThisKey = nameof(Id3) + "," + nameof(Id3), OtherKey = nameof(Testfkunique.Id3) + "," + nameof(Id3))]
		public IEnumerable<Testfkunique> Testfkuniques1 { get; set; } = null!;
		#endregion
	}

	[Table("testfkunique", Schema = "informix")]
	public partial class Testfkunique
	{
		[Column("id1")] public int Id1 { get; set; } // INTEGER
		[Column("id2")] public int Id2 { get; set; } // INTEGER
		[Column("id3")] public int Id3 { get; set; } // INTEGER
		[Column("id4")] public int Id4 { get; set; } // INTEGER

		#region Associations
		/// <summary>
		/// FK_testfkunique_testunique
		/// </summary>
		[Association(CanBeNull = false, ThisKey = nameof(Id1) + "," + nameof(Id1), OtherKey = nameof(Informix.Testunique.Id1) + "," + nameof(Id1))]
		public Testunique Testunique { get; set; } = null!;

		/// <summary>
		/// FK_testfkunique_testunique_1
		/// </summary>
		[Association(CanBeNull = false, ThisKey = nameof(Id3) + "," + nameof(Id3), OtherKey = nameof(Informix.Testunique.Id3) + "," + nameof(Id3))]
		public Testunique Testunique1 { get; set; } = null!;
		#endregion
	}

	[Table("testmerge1", Schema = "informix")]
	public partial class Testmerge1
	{
		[Column("id"             , IsPrimaryKey = true)] public int       Id              { get; set; } // INTEGER
		[Column("field1"                              )] public int?      Field1          { get; set; } // INTEGER
		[Column("field2"                              )] public int?      Field2          { get; set; } // INTEGER
		[Column("field3"                              )] public int?      Field3          { get; set; } // INTEGER
		[Column("field4"                              )] public int?      Field4          { get; set; } // INTEGER
		[Column("field5"                              )] public int?      Field5          { get; set; } // INTEGER
		[Column("fieldint64"                          )] public long?     Fieldint64      { get; set; } // BIGINT
		[Column("fieldboolean"                        )] public bool?     Fieldboolean    { get; set; } // BOOLEAN
		[Column("fieldstring"                         )] public string?   Fieldstring     { get; set; } // VARCHAR(20)
		[Column("fieldchar"                           )] public char?     Fieldchar       { get; set; } // CHAR(1)
		[Column("fieldfloat"                          )] public float?    Fieldfloat      { get; set; } // SMALLFLOAT
		[Column("fielddouble"                         )] public double?   Fielddouble     { get; set; } // FLOAT
		[Column("fielddatetime"                       )] public DateTime? Fielddatetime   { get; set; } // DATETIME YEAR TO FRACTION(3)
		[Column("fieldbinary"                         )] public byte[]?   Fieldbinary     { get; set; } // BYTE
		[Column("fielddecimal"                        )] public decimal?  Fielddecimal    { get; set; } // DECIMAL(24,10)
		[Column("fielddate"                           )] public DateTime? Fielddate       { get; set; } // DATE
		[Column("fieldtime"                           )] public TimeSpan? Fieldtime       { get; set; } // INTERVAL HOUR TO FRACTION(5)
		[Column("fieldenumstring"                     )] public string?   Fieldenumstring { get; set; } // VARCHAR(20)
		[Column("fieldenumnumber"                     )] public int?      Fieldenumnumber { get; set; } // INTEGER
	}

	[Table("testmerge2", Schema = "informix")]
	public partial class Testmerge2
	{
		[Column("id"             , IsPrimaryKey = true)] public int       Id              { get; set; } // INTEGER
		[Column("field1"                              )] public int?      Field1          { get; set; } // INTEGER
		[Column("field2"                              )] public int?      Field2          { get; set; } // INTEGER
		[Column("field3"                              )] public int?      Field3          { get; set; } // INTEGER
		[Column("field4"                              )] public int?      Field4          { get; set; } // INTEGER
		[Column("field5"                              )] public int?      Field5          { get; set; } // INTEGER
		[Column("fieldint64"                          )] public long?     Fieldint64      { get; set; } // BIGINT
		[Column("fieldboolean"                        )] public bool?     Fieldboolean    { get; set; } // BOOLEAN
		[Column("fieldstring"                         )] public string?   Fieldstring     { get; set; } // VARCHAR(20)
		[Column("fieldchar"                           )] public char?     Fieldchar       { get; set; } // CHAR(1)
		[Column("fieldfloat"                          )] public float?    Fieldfloat      { get; set; } // SMALLFLOAT
		[Column("fielddouble"                         )] public double?   Fielddouble     { get; set; } // FLOAT
		[Column("fielddatetime"                       )] public DateTime? Fielddatetime   { get; set; } // DATETIME YEAR TO FRACTION(3)
		[Column("fieldbinary"                         )] public byte[]?   Fieldbinary     { get; set; } // BYTE
		[Column("fielddecimal"                        )] public decimal?  Fielddecimal    { get; set; } // DECIMAL(24,10)
		[Column("fielddate"                           )] public DateTime? Fielddate       { get; set; } // DATE
		[Column("fieldtime"                           )] public TimeSpan? Fieldtime       { get; set; } // INTERVAL HOUR TO FRACTION(5)
		[Column("fieldenumstring"                     )] public string?   Fieldenumstring { get; set; } // VARCHAR(20)
		[Column("fieldenumnumber"                     )] public int?      Fieldenumnumber { get; set; } // INTEGER
	}

	[Table("collatedtable", Schema = "informix")]
	public partial class Collatedtable
	{
		[Column("id"                                )] public int    Id              { get; set; } // INTEGER
		[Column("casesensitive"  , CanBeNull = false)] public string Casesensitive   { get; set; } = null!; // VARCHAR(20)
		[Column("caseinsensitive", CanBeNull = false)] public string Caseinsensitive { get; set; } = null!; // NVARCHAR(20)
	}

	[Table("personview", Schema = "informix", IsView = true)]
	public partial class Personview
	{
		[Column("personid"  , IsIdentity = true , SkipOnInsert = true, SkipOnUpdate = true)] public int     Personid   { get; set; } // SERIAL
		[Column("firstname" , CanBeNull  = false                                          )] public string  Firstname  { get; set; } = null!; // NVARCHAR(50)
		[Column("lastname"  , CanBeNull  = false                                          )] public string  Lastname   { get; set; } = null!; // NVARCHAR(50)
		[Column("middlename"                                                              )] public string? Middlename { get; set; } // NVARCHAR(50)
		[Column("gender"                                                                  )] public char    Gender     { get; set; } // CHAR(1)
	}
}
