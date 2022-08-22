﻿using System;

namespace DBLayer
{
	public partial class Sql
	{
		public enum QueryExtensionScope
		{
			None,
			TableHint,
			TablesInScopeHint,
			IndexHint,
			JoinHint,
			SubQueryHint,
			QueryHint
		}
	}
}