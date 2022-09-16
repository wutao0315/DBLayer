// ---------------------------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by DBLayer scaffolding tool (https://github.com/linq2db/linq2db).
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
// ---------------------------------------------------------------------------------------------------

using DBLayer;
using DBLayer.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable 1573, 1591
#nullable enable

namespace Cli.Default.DB2
{
	public static partial class Db2Inst1Schema
	{
		public partial class DataContext
		{
			private readonly IDataContext _dataContext;

			public ITable<Alltype>           Alltypes            => _dataContext.GetTable<Alltype>();
			public ITable<Child>             Children            => _dataContext.GetTable<Child>();
			public ITable<CollatedTable>     CollatedTables      => _dataContext.GetTable<CollatedTable>();
			public ITable<Doctor>            Doctors             => _dataContext.GetTable<Doctor>();
			public ITable<GrandChild>        GrandChildren       => _dataContext.GetTable<GrandChild>();
			public ITable<InheritanceChild>  InheritanceChildren => _dataContext.GetTable<InheritanceChild>();
			public ITable<InheritanceParent> InheritanceParents  => _dataContext.GetTable<InheritanceParent>();
			public ITable<KeepIdentityTest>  KeepIdentityTests   => _dataContext.GetTable<KeepIdentityTest>();
			public ITable<LinqDataType>      LinqDataTypes       => _dataContext.GetTable<LinqDataType>();
			public ITable<Mastertable>       Mastertables        => _dataContext.GetTable<Mastertable>();
			public ITable<Parent>            Parents             => _dataContext.GetTable<Parent>();
			public ITable<Patient>           Patients            => _dataContext.GetTable<Patient>();
			public ITable<Person>            People              => _dataContext.GetTable<Person>();
			public ITable<Slavetable>        Slavetables         => _dataContext.GetTable<Slavetable>();
			public ITable<TestIdentity>      TestIdentities      => _dataContext.GetTable<TestIdentity>();
			public ITable<TestMerge1>        TestMerge1          => _dataContext.GetTable<TestMerge1>();
			public ITable<TestMerge2>        TestMerge2          => _dataContext.GetTable<TestMerge2>();
			public ITable<Personview>        Personviews         => _dataContext.GetTable<Personview>();

			public DataContext(IDataContext dataContext)
			{
				_dataContext = dataContext;
			}
		}

		#region Table Extensions
		public static Alltype? Find(this ITable<Alltype> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<Alltype?> FindAsync(this ITable<Alltype> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static Doctor? Find(this ITable<Doctor> table, int personId)
		{
			return table.FirstOrDefault(e => e.PersonId == personId);
		}

		public static Task<Doctor?> FindAsync(this ITable<Doctor> table, int personId, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.PersonId == personId, cancellationToken);
		}

		public static InheritanceChild? Find(this ITable<InheritanceChild> table, int inheritanceChildId)
		{
			return table.FirstOrDefault(e => e.InheritanceChildId == inheritanceChildId);
		}

		public static Task<InheritanceChild?> FindAsync(this ITable<InheritanceChild> table, int inheritanceChildId, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.InheritanceChildId == inheritanceChildId, cancellationToken);
		}

		public static InheritanceParent? Find(this ITable<InheritanceParent> table, int inheritanceParentId)
		{
			return table.FirstOrDefault(e => e.InheritanceParentId == inheritanceParentId);
		}

		public static Task<InheritanceParent?> FindAsync(this ITable<InheritanceParent> table, int inheritanceParentId, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.InheritanceParentId == inheritanceParentId, cancellationToken);
		}

		public static KeepIdentityTest? Find(this ITable<KeepIdentityTest> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<KeepIdentityTest?> FindAsync(this ITable<KeepIdentityTest> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static Mastertable? Find(this ITable<Mastertable> table, int id1, int id2)
		{
			return table.FirstOrDefault(e => e.Id1 == id1 && e.Id2 == id2);
		}

		public static Task<Mastertable?> FindAsync(this ITable<Mastertable> table, int id1, int id2, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id1 == id1 && e.Id2 == id2, cancellationToken);
		}

		public static Patient? Find(this ITable<Patient> table, int personId)
		{
			return table.FirstOrDefault(e => e.PersonId == personId);
		}

		public static Task<Patient?> FindAsync(this ITable<Patient> table, int personId, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.PersonId == personId, cancellationToken);
		}

		public static Person? Find(this ITable<Person> table, int personId)
		{
			return table.FirstOrDefault(e => e.PersonId == personId);
		}

		public static Task<Person?> FindAsync(this ITable<Person> table, int personId, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.PersonId == personId, cancellationToken);
		}

		public static TestIdentity? Find(this ITable<TestIdentity> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<TestIdentity?> FindAsync(this ITable<TestIdentity> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static TestMerge1? Find(this ITable<TestMerge1> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<TestMerge1?> FindAsync(this ITable<TestMerge1> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static TestMerge2? Find(this ITable<TestMerge2> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<TestMerge2?> FindAsync(this ITable<TestMerge2> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}
		#endregion

		[Table("ALLTYPES", Schema = "DB2INST1")]
		public class Alltype
		{
			[Column("ID"               , IsPrimaryKey = true, IsIdentity = true, SkipOnInsert = true, SkipOnUpdate = true)] public int       Id                { get; set; } // INTEGER
			[Column("BIGINTDATATYPE"                                                                                     )] public long?     Bigintdatatype    { get; set; } // BIGINT
			[Column("INTDATATYPE"                                                                                        )] public int?      Intdatatype       { get; set; } // INTEGER
			[Column("SMALLINTDATATYPE"                                                                                   )] public short?    Smallintdatatype  { get; set; } // SMALLINT
			[Column("DECIMALDATATYPE"                                                                                    )] public decimal?  Decimaldatatype   { get; set; } // DECIMAL
			[Column("DECFLOATDATATYPE"                                                                                   )] public decimal?  Decfloatdatatype  { get; set; } // DECFLOAT(16)
			[Column("REALDATATYPE"                                                                                       )] public float?    Realdatatype      { get; set; } // REAL
			[Column("DOUBLEDATATYPE"                                                                                     )] public double?   Doubledatatype    { get; set; } // DOUBLE
			[Column("CHARDATATYPE"                                                                                       )] public char?     Chardatatype      { get; set; } // CHARACTER(1)
			[Column("CHAR20DATATYPE"                                                                                     )] public string?   Char20Datatype    { get; set; } // CHARACTER(20)
			[Column("VARCHARDATATYPE"                                                                                    )] public string?   Varchardatatype   { get; set; } // VARCHAR(20)
			[Column("CLOBDATATYPE"                                                                                       )] public string?   Clobdatatype      { get; set; } // CLOB(1048576)
			[Column("DBCLOBDATATYPE"                                                                                     )] public string?   Dbclobdatatype    { get; set; } // DBCLOB(100)
			[Column("BINARYDATATYPE"                                                                                     )] public byte[]?   Binarydatatype    { get; set; } // CHAR (5) FOR BIT DATA
			[Column("VARBINARYDATATYPE"                                                                                  )] public byte[]?   Varbinarydatatype { get; set; } // VARCHAR (5) FOR BIT DATA
			[Column("BLOBDATATYPE"                                                                                       )] public byte[]?   Blobdatatype      { get; set; } // BLOB(1048576)
			[Column("GRAPHICDATATYPE"                                                                                    )] public string?   Graphicdatatype   { get; set; } // GRAPHIC(10)
			[Column("DATEDATATYPE"                                                                                       )] public DateTime? Datedatatype      { get; set; } // DATE
			[Column("TIMEDATATYPE"                                                                                       )] public TimeSpan? Timedatatype      { get; set; } // TIME
			[Column("TIMESTAMPDATATYPE"                                                                                  )] public DateTime? Timestampdatatype { get; set; } // TIMESTAMP
			[Column("XMLDATATYPE"                                                                                        )] public string?   Xmldatatype       { get; set; } // XML
		}

		[Table("Child", Schema = "DB2INST1")]
		public class Child
		{
			[Column("ParentID")] public int? ParentId { get; set; } // INTEGER
			[Column("ChildID" )] public int? ChildId  { get; set; } // INTEGER
		}

		[Table("CollatedTable", Schema = "DB2INST1")]
		public class CollatedTable
		{
			[Column("Id"                                )] public int    Id              { get; set; } // INTEGER
			[Column("CaseSensitive"  , CanBeNull = false)] public string CaseSensitive   { get; set; } = null!; // VARCHAR(80)
			[Column("CaseInsensitive", CanBeNull = false)] public string CaseInsensitive { get; set; } = null!; // VARCHAR(80)
		}

		[Table("Doctor", Schema = "DB2INST1")]
		public class Doctor
		{
			[Column("PersonID", IsPrimaryKey = true )] public int    PersonId { get; set; } // INTEGER
			[Column("Taxonomy", CanBeNull    = false)] public string Taxonomy { get; set; } = null!; // VARCHAR(50)

			#region Associations
			/// <summary>
			/// FK_Doctor_Person
			/// </summary>
			[Association(CanBeNull = false, ThisKey = nameof(PersonId), OtherKey = nameof(Db2Inst1Schema.Person.PersonId))]
			public Person Person { get; set; } = null!;
			#endregion
		}

		[Table("GrandChild", Schema = "DB2INST1")]
		public class GrandChild
		{
			[Column("ParentID"    )] public int? ParentId     { get; set; } // INTEGER
			[Column("ChildID"     )] public int? ChildId      { get; set; } // INTEGER
			[Column("GrandChildID")] public int? GrandChildId { get; set; } // INTEGER
		}

		[Table("InheritanceChild", Schema = "DB2INST1")]
		public class InheritanceChild
		{
			[Column("InheritanceChildId" , IsPrimaryKey = true)] public int     InheritanceChildId  { get; set; } // INTEGER
			[Column("InheritanceParentId"                     )] public int     InheritanceParentId { get; set; } // INTEGER
			[Column("TypeDiscriminator"                       )] public int?    TypeDiscriminator   { get; set; } // INTEGER
			[Column("Name"                                    )] public string? Name                { get; set; } // VARCHAR(50)
		}

		[Table("InheritanceParent", Schema = "DB2INST1")]
		public class InheritanceParent
		{
			[Column("InheritanceParentId", IsPrimaryKey = true)] public int     InheritanceParentId { get; set; } // INTEGER
			[Column("TypeDiscriminator"                       )] public int?    TypeDiscriminator   { get; set; } // INTEGER
			[Column("Name"                                    )] public string? Name                { get; set; } // VARCHAR(50)
		}

		[Table("KeepIdentityTest", Schema = "DB2INST1")]
		public class KeepIdentityTest
		{
			[Column("ID"   , IsPrimaryKey = true, IsIdentity = true, SkipOnInsert = true, SkipOnUpdate = true)] public int  Id    { get; set; } // INTEGER
			[Column("Value"                                                                                  )] public int? Value { get; set; } // INTEGER
		}

		[Table("LinqDataTypes", Schema = "DB2INST1")]
		public class LinqDataType
		{
			[Column("ID"            )] public int?      Id             { get; set; } // INTEGER
			[Column("MoneyValue"    )] public decimal?  MoneyValue     { get; set; } // DECIMAL(10,4)
			[Column("DateTimeValue" )] public DateTime? DateTimeValue  { get; set; } // TIMESTAMP
			[Column("DateTimeValue2")] public DateTime? DateTimeValue2 { get; set; } // TIMESTAMP
			[Column("BoolValue"     )] public short?    BoolValue      { get; set; } // SMALLINT
			[Column("GuidValue"     )] public byte[]?   GuidValue      { get; set; } // CHAR (16) FOR BIT DATA
			[Column("BinaryValue"   )] public byte[]?   BinaryValue    { get; set; } // BLOB(5000)
			[Column("SmallIntValue" )] public short?    SmallIntValue  { get; set; } // SMALLINT
			[Column("IntValue"      )] public int?      IntValue       { get; set; } // INTEGER
			[Column("BigIntValue"   )] public long?     BigIntValue    { get; set; } // BIGINT
			[Column("StringValue"   )] public string?   StringValue    { get; set; } // VARCHAR(50)
		}

		[Table("MASTERTABLE", Schema = "DB2INST1")]
		public class Mastertable
		{
			[Column("ID1", IsPrimaryKey = true, PrimaryKeyOrder = 0)] public int Id1 { get; set; } // INTEGER
			[Column("ID2", IsPrimaryKey = true, PrimaryKeyOrder = 1)] public int Id2 { get; set; } // INTEGER

			#region Associations
			/// <summary>
			/// FK_SLAVETABLE_MASTERTABLE backreference
			/// </summary>
			[Association(ThisKey = nameof(Id1) + "," + nameof(Id1), OtherKey = nameof(Slavetable.Id222222222222222222222222) + "," + nameof(Id1))]
			public IEnumerable<Slavetable> Slavetables { get; set; } = null!;
			#endregion
		}

		[Table("Parent", Schema = "DB2INST1")]
		public class Parent
		{
			[Column("ParentID")] public int? ParentId { get; set; } // INTEGER
			[Column("Value1"  )] public int? Value1   { get; set; } // INTEGER
		}

		[Table("Patient", Schema = "DB2INST1")]
		public class Patient
		{
			[Column("PersonID" , IsPrimaryKey = true )] public int    PersonId  { get; set; } // INTEGER
			[Column("Diagnosis", CanBeNull    = false)] public string Diagnosis { get; set; } = null!; // VARCHAR(256)

			#region Associations
			/// <summary>
			/// FK_Patient_Person
			/// </summary>
			[Association(CanBeNull = false, ThisKey = nameof(PersonId), OtherKey = nameof(Db2Inst1Schema.Person.PersonId))]
			public Person Person { get; set; } = null!;
			#endregion
		}

		[Table("Person", Schema = "DB2INST1")]
		public class Person
		{
			[Column("PersonID"  , IsPrimaryKey = true , IsIdentity = true, SkipOnInsert = true, SkipOnUpdate = true)] public int     PersonId   { get; set; } // INTEGER
			[Column("FirstName" , CanBeNull    = false                                                             )] public string  FirstName  { get; set; } = null!; // VARCHAR(50)
			[Column("LastName"  , CanBeNull    = false                                                             )] public string  LastName   { get; set; } = null!; // VARCHAR(50)
			[Column("MiddleName"                                                                                   )] public string? MiddleName { get; set; } // VARCHAR(50)
			[Column("Gender"                                                                                       )] public char    Gender     { get; set; } // CHARACTER(1)

			#region Associations
			/// <summary>
			/// FK_Doctor_Person backreference
			/// </summary>
			[Association(ThisKey = nameof(PersonId), OtherKey = nameof(Db2Inst1Schema.Doctor.PersonId))]
			public Doctor? Doctor { get; set; }

			/// <summary>
			/// FK_Patient_Person backreference
			/// </summary>
			[Association(ThisKey = nameof(PersonId), OtherKey = nameof(Db2Inst1Schema.Patient.PersonId))]
			public Patient? Patient { get; set; }
			#endregion
		}

		[Table("SLAVETABLE", Schema = "DB2INST1")]
		public class Slavetable
		{
			[Column("ID1"                          )] public int Id1                        { get; set; } // INTEGER
			[Column("ID 2222222222222222222222  22")] public int Id222222222222222222222222 { get; set; } // INTEGER
			[Column("ID 2222222222222222"          )] public int Id2222222222222222         { get; set; } // INTEGER

			#region Associations
			/// <summary>
			/// FK_SLAVETABLE_MASTERTABLE
			/// </summary>
			[Association(CanBeNull = false, ThisKey = nameof(Id222222222222222222222222) + "," + nameof(Id222222222222222222222222), OtherKey = nameof(Db2Inst1Schema.Mastertable.Id1) + "," + nameof(Id222222222222222222222222))]
			public Mastertable Mastertable { get; set; } = null!;
			#endregion
		}

		[Table("TestIdentity", Schema = "DB2INST1")]
		public class TestIdentity
		{
			[Column("ID", IsPrimaryKey = true, IsIdentity = true, SkipOnInsert = true, SkipOnUpdate = true)] public int Id { get; set; } // INTEGER
		}

		[Table("TestMerge1", Schema = "DB2INST1")]
		public class TestMerge1
		{
			[Column("Id"             , IsPrimaryKey = true)] public int       Id              { get; set; } // INTEGER
			[Column("Field1"                              )] public int?      Field1          { get; set; } // INTEGER
			[Column("Field2"                              )] public int?      Field2          { get; set; } // INTEGER
			[Column("Field3"                              )] public int?      Field3          { get; set; } // INTEGER
			[Column("Field4"                              )] public int?      Field4          { get; set; } // INTEGER
			[Column("Field5"                              )] public int?      Field5          { get; set; } // INTEGER
			[Column("FieldInt64"                          )] public long?     FieldInt64      { get; set; } // BIGINT
			[Column("FieldBoolean"                        )] public short?    FieldBoolean    { get; set; } // SMALLINT
			[Column("FieldString"                         )] public string?   FieldString     { get; set; } // VARCHAR(20)
			[Column("FieldNString"                        )] public string?   FieldNString    { get; set; } // VARCHAR(80)
			[Column("FieldChar"                           )] public char?     FieldChar       { get; set; } // CHARACTER(1)
			[Column("FieldNChar"                          )] public string?   FieldNChar      { get; set; } // CHARACTER(4)
			[Column("FieldFloat"                          )] public float?    FieldFloat      { get; set; } // REAL
			[Column("FieldDouble"                         )] public double?   FieldDouble     { get; set; } // DOUBLE
			[Column("FieldDateTime"                       )] public DateTime? FieldDateTime   { get; set; } // TIMESTAMP
			[Column("FieldBinary"                         )] public byte[]?   FieldBinary     { get; set; } // VARCHAR (20) FOR BIT DATA
			[Column("FieldGuid"                           )] public byte[]?   FieldGuid       { get; set; } // CHAR (16) FOR BIT DATA
			[Column("FieldDecimal"                        )] public decimal?  FieldDecimal    { get; set; } // DECIMAL(24,10)
			[Column("FieldDate"                           )] public DateTime? FieldDate       { get; set; } // DATE
			[Column("FieldTime"                           )] public TimeSpan? FieldTime       { get; set; } // TIME
			[Column("FieldEnumString"                     )] public string?   FieldEnumString { get; set; } // VARCHAR(20)
			[Column("FieldEnumNumber"                     )] public int?      FieldEnumNumber { get; set; } // INTEGER
		}

		[Table("TestMerge2", Schema = "DB2INST1")]
		public class TestMerge2
		{
			[Column("Id"             , IsPrimaryKey = true)] public int       Id              { get; set; } // INTEGER
			[Column("Field1"                              )] public int?      Field1          { get; set; } // INTEGER
			[Column("Field2"                              )] public int?      Field2          { get; set; } // INTEGER
			[Column("Field3"                              )] public int?      Field3          { get; set; } // INTEGER
			[Column("Field4"                              )] public int?      Field4          { get; set; } // INTEGER
			[Column("Field5"                              )] public int?      Field5          { get; set; } // INTEGER
			[Column("FieldInt64"                          )] public long?     FieldInt64      { get; set; } // BIGINT
			[Column("FieldBoolean"                        )] public short?    FieldBoolean    { get; set; } // SMALLINT
			[Column("FieldString"                         )] public string?   FieldString     { get; set; } // VARCHAR(20)
			[Column("FieldNString"                        )] public string?   FieldNString    { get; set; } // VARCHAR(80)
			[Column("FieldChar"                           )] public char?     FieldChar       { get; set; } // CHARACTER(1)
			[Column("FieldNChar"                          )] public string?   FieldNChar      { get; set; } // CHARACTER(4)
			[Column("FieldFloat"                          )] public float?    FieldFloat      { get; set; } // REAL
			[Column("FieldDouble"                         )] public double?   FieldDouble     { get; set; } // DOUBLE
			[Column("FieldDateTime"                       )] public DateTime? FieldDateTime   { get; set; } // TIMESTAMP
			[Column("FieldBinary"                         )] public byte[]?   FieldBinary     { get; set; } // VARCHAR (20) FOR BIT DATA
			[Column("FieldGuid"                           )] public byte[]?   FieldGuid       { get; set; } // CHAR (16) FOR BIT DATA
			[Column("FieldDecimal"                        )] public decimal?  FieldDecimal    { get; set; } // DECIMAL(24,10)
			[Column("FieldDate"                           )] public DateTime? FieldDate       { get; set; } // DATE
			[Column("FieldTime"                           )] public TimeSpan? FieldTime       { get; set; } // TIME
			[Column("FieldEnumString"                     )] public string?   FieldEnumString { get; set; } // VARCHAR(20)
			[Column("FieldEnumNumber"                     )] public int?      FieldEnumNumber { get; set; } // INTEGER
		}

		[Table("PERSONVIEW", Schema = "DB2INST1", IsView = true)]
		public class Personview
		{
			[Column("PersonID"                     )] public int     PersonId   { get; set; } // INTEGER
			[Column("FirstName" , CanBeNull = false)] public string  FirstName  { get; set; } = null!; // VARCHAR(50)
			[Column("LastName"  , CanBeNull = false)] public string  LastName   { get; set; } = null!; // VARCHAR(50)
			[Column("MiddleName"                   )] public string? MiddleName { get; set; } // VARCHAR(50)
			[Column("Gender"                       )] public char    Gender     { get; set; } // CHARACTER(1)
		}
	}
}
