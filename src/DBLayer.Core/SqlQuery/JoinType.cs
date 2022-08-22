using System;

namespace DBLayer.SqlQuery
{
	public enum JoinType
	{
		Auto,
		Inner,
		Left,
		CrossApply,
		OuterApply,
		Right,
		Full
	}
}
