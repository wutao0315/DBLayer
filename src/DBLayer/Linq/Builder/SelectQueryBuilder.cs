﻿using DBLayer.Expressions;
using System.Linq.Expressions;

namespace DBLayer.Linq.Builder;
class SelectQueryBuilder : MethodCallBuilder
{
	protected override bool CanBuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
	{
		return methodCall.IsSameGenericMethod(DataExtensions.SelectQueryMethodInfo);
	}

	protected override IBuildContext BuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
	{
		var sequence = new SelectContext(buildInfo.Parent, builder,
			(LambdaExpression)methodCall.Arguments[1].Unwrap(),
			buildInfo.SelectQuery);

		return sequence;
	}

	public override bool IsSequence(ExpressionBuilder builder, BuildInfo buildInfo)
	{
		return true;
	}
}