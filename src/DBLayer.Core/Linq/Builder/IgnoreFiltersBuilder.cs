using System;
using System.Linq.Expressions;
using DBLayer.Expressions;
using DBLayer.Reflection;

namespace DBLayer.Linq.Builder;

class IgnoreFiltersBuilder : MethodCallBuilder
{
	protected override bool CanBuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
	{
		return methodCall.IsSameGenericMethod(Methods.DBLayer.IgnoreFilters);
	}

	protected override IBuildContext BuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
	{
		var types = (Type[])methodCall.Arguments[1].EvaluateExpression()!;

		builder.PushDisabledQueryFilters(types);
		var sequence = builder.BuildSequence(new BuildInfo(buildInfo, methodCall.Arguments[0]));
		builder.PopDisabledFilter();

		return sequence;
	}
}
