﻿using System.Linq.Expressions;
using DBLayer.Expressions;

namespace DBLayer.Linq.Builder;

class AsUpdatableBuilder : MethodCallBuilder
{
	protected override bool CanBuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
	{
		return methodCall.IsQueryable("AsUpdatable");
	}

	protected override IBuildContext BuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
	{
		return builder.BuildSequence(new BuildInfo(buildInfo, methodCall.Arguments[0]));
	}
}