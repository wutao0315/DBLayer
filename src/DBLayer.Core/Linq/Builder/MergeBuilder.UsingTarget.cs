using System.Linq.Expressions;
using DBLayer.Expressions;
using static DBLayer.Reflection.Methods.DBLayer.Merge;

namespace DBLayer.Linq.Builder;

internal partial class MergeBuilder
{
	internal class UsingTarget : MethodCallBuilder
	{
		protected override bool CanBuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			return methodCall.IsSameGenericMethod(UsingTargetMethodInfo);
		}

		protected override IBuildContext BuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			var mergeContext = (MergeContext)builder.BuildSequence(new BuildInfo(buildInfo, methodCall.Arguments[0]));

			// is it ok to reuse context like that?
			var source                = new TableLikeQueryContext(mergeContext.TargetContext);
			mergeContext.Sequences    = new IBuildContext[] { mergeContext.Sequence, source };
			mergeContext.Merge.Source = source.Source;

			return mergeContext;
		}
	}
}
