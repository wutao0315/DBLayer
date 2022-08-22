using DBLayer.Expressions;
using DBLayer.Reflection;
using System.Linq.Expressions;
using System.Reflection;

namespace DBLayer.Linq.Builder;
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
