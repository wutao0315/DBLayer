using System;
using System.Reflection;
using System.Linq.Expressions;

namespace DBLayer.Linq.Builder
{
	using DBLayer.Expressions;
	using DBLayer.Reflection;

	class PassThroughBuilder : MethodCallBuilder
	{
		static readonly MethodInfo[] _supportedMethods = { Methods.Enumerable.AsQueryable, Methods.DBLayer.AsQueryable, Methods.DBLayer.SqlExt.Alias };

		protected override bool CanBuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			return methodCall.IsSameGenericMethod(_supportedMethods);
		}

		protected override IBuildContext BuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			return builder.BuildSequence(new BuildInfo(buildInfo, methodCall.Arguments[0]));
		}
	}
}
