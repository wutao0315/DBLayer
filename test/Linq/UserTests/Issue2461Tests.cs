using System.Linq;
using DBLayer;
using NUnit.Framework;

namespace Tests.UserTests
{
	[TestFixture]
	public class Issue2461Tests : TestBase
	{
		[DBLayer.Mapping.Table("MRECEIPT")]
		public class TestReceipt
		{
			public static string TableName => "MRECEIPT";
			public static string ExternalReceiptsTableName => "EXTERNAL_RECEIPTS";

			[DBLayer.Mapping.Column("RECEIPT_NO")]            public string           ReceiptNo            { get; set; } = null!;
			[DBLayer.Mapping.Column("CUSTKEY")]               public string           Custkey              { get; set; } = null!;

			[DBLayer.Mapping.Association(ThisKey=nameof(Custkey), OtherKey=nameof(TestCustomer.Custkey), CanBeNull=true)]
			public TestCustomer Customer { get; } = null!;
		}

		[DBLayer.Mapping.Table("CUST_DTL")]
		public class TestCustomer
		{
			[DBLayer.Mapping.Column("CUSTKEY")]
			public string Custkey { get; set;} = null!;

			[DBLayer.Mapping.Column("BILLGROUP")] public string BillingGroup { get; set; } = null!;

			public static string GetName(string name) => name;
		}

		[Test]
		public void AssociationConcat([DataSources] string context)
		{
			using (var db = GetDataContext(context))
			using (db.CreateLocalTable<TestReceipt>())
			using (db.CreateLocalTable<TestCustomer>())
			{
				var query = db.GetTable<TestReceipt>()
					.Concat(db.GetTable<TestReceipt>().TableName(TestReceipt.ExternalReceiptsTableName))
					.Select(
						i =>
							new { i.ReceiptNo, a = TestCustomer.GetName(i.Customer.BillingGroup) });

				Assert.Throws<DBLayerException>(() => _ = query.ToArray(),
					"Associations with Concat/Union or other Set operations are not supported.");
			}
		}
	}
}
