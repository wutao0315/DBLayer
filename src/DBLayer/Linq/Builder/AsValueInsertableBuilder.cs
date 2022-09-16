﻿using DBLayer.Expressions;
using System.Linq.Expressions;

namespace DBLayer.Linq.Builder;

class AsValueInsertableBuilder : MethodCallBuilder
{
	protected override bool CanBuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
	{
		return methodCall.IsQueryable("AsValueInsertable");
	}

	protected override IBuildContext BuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
	{
		return builder.BuildSequence(new BuildInfo(buildInfo, methodCall.Arguments[0]));
	}

	protected override SequenceConvertInfo? Convert(
		ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo, ParameterExpression? param)
	{
		return null;
	}
}