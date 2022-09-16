﻿using DBLayer;
using DBLayer.Mapping;
using NUnit.Framework;
using System.Linq;

namespace Tests.UserTests
{
	[TestFixture]
	public class Issue1279Tests : TestBase
	{
		class Issue1279Table
		{
			[PrimaryKey(1)]
			[Identity] public int Id { get; set; }

			public char CharFld { get; set; }
		}

		[Test]
		public void Test([DataSources] string context)
		{
			using (var db = GetDataContext(context))
			using (var table = db.CreateLocalTable<Issue1279Table>())
			{
				var val = 'P';

				db.Insert(new Issue1279Table { CharFld = val });

				var result = db.GetTable<Issue1279Table>().First().CharFld;

				Assert.AreEqual(val, result);
			}
		}
	}
}