using System;
using System.Linq.Expressions;

namespace DBLayer.Linq.Builder
{
	using DBLayer.Expressions;
	using Reflection;

	class RemoveOrderByBuilder : MethodCallBuilder
	{
		protected override bool CanBuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			return methodCall.IsSameGenericMethod(Methods.DBLayer.RemoveOrderBy);
		}

		protected override IBuildContext BuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			var sequence = builder.BuildSequence(new BuildInfo(buildInfo, methodCall.Arguments[0]));

			if (sequence.SelectQuery.Select.TakeValue == null &&
			    sequence.SelectQuery.Select.SkipValue == null)
			{
				sequence.SelectQuery.OrderBy.Items.Clear();
			}

			return sequence;
		}
	}
}
