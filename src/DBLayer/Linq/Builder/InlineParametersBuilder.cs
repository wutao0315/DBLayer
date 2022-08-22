using System.Linq.Expressions;
using DBLayer.Expressions;
using DBLayer.Reflection;

namespace DBLayer.Linq.Builder;
class InlineParametersBuilder : MethodCallBuilder
{
	protected override bool CanBuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
	{
		return methodCall.IsSameGenericMethod(Methods.DBLayer.InlineParameters);
	}

	protected override IBuildContext BuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
	{
		var saveInline = builder.DataContext.InlineParameters;
		builder.DataContext.InlineParameters = true;

		var sequence = builder.BuildSequence(new BuildInfo(buildInfo, methodCall.Arguments[0]));

		builder.DataContext.InlineParameters = saveInline;

		return sequence;
	}
}
