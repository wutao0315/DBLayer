// ---------------------------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by DBLayer scaffolding tool (https://github.com/linq2db/linq2db).
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
// ---------------------------------------------------------------------------------------------------

using DBLayer;
using DBLayer.Configuration;
using DBLayer.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

#pragma warning disable 1573, 1591
#nullable enable

namespace Cli.Default.Informix
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

	public static partial class ExtensionMethods
	{
		#region Table Extensions
		public static Inheritanceparent? Find(this ITable<Inheritanceparent> table, int inheritanceparentid)
		{
			return table.FirstOrDefault(e => e.Inheritanceparentid == inheritanceparentid);
		}

		public static Task<Inheritanceparent?> FindAsync(this ITable<Inheritanceparent> table, int inheritanceparentid, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Inheritanceparentid == inheritanceparentid, cancellationToken);
		}

		public static Inheritancechild? Find(this ITable<Inheritancechild> table, int inheritancechildid)
		{
			return table.FirstOrDefault(e => e.Inheritancechildid == inheritancechildid);
		}

		public static Task<Inheritancechild?> FindAsync(this ITable<Inheritancechild> table, int inheritancechildid, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Inheritancechildid == inheritancechildid, cancellationToken);
		}

		public static Person? Find(this ITable<Person> table, int personid)
		{
			return table.FirstOrDefault(e => e.Personid == personid);
		}

		public static Task<Person?> FindAsync(this ITable<Person> table, int personid, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Personid == personid, cancellationToken);
		}

		public static Doctor? Find(this ITable<Doctor> table, int personid)
		{
			return table.FirstOrDefault(e => e.Personid == personid);
		}

		public static Task<Doctor?> FindAsync(this ITable<Doctor> table, int personid, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Personid == personid, cancellationToken);
		}

		public static Patient? Find(this ITable<Patient> table, int personid)
		{
			return table.FirstOrDefault(e => e.Personid == personid);
		}

		public static Task<Patient?> FindAsync(this ITable<Patient> table, int personid, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Personid == personid, cancellationToken);
		}

		public static Testidentity? Find(this ITable<Testidentity> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<Testidentity?> FindAsync(this ITable<Testidentity> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static Alltype? Find(this ITable<Alltype> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<Alltype?> FindAsync(this ITable<Alltype> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static Testunique? Find(this ITable<Testunique> table, int id1, int id2)
		{
			return table.FirstOrDefault(e => e.Id1 == id1 && e.Id2 == id2);
		}

		public static Task<Testunique?> FindAsync(this ITable<Testunique> table, int id1, int id2, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id1 == id1 && e.Id2 == id2, cancellationToken);
		}

		public static Testmerge1? Find(this ITable<Testmerge1> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<Testmerge1?> FindAsync(this ITable<Testmerge1> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}

		public static Testmerge2? Find(this ITable<Testmerge2> table, int id)
		{
			return table.FirstOrDefault(e => e.Id == id);
		}

		public static Task<Testmerge2?> FindAsync(this ITable<Testmerge2> table, int id, CancellationToken cancellationToken = default)
		{
			return table.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
		}
		#endregion
	}
}
