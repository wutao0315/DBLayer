using System.Linq.Expressions;
using DBLayer.Reflection;
using DBLayer.Expressions;
using System.Reflection;

namespace DBLayer.Linq.Builder;

class DistinctBuilder : MethodCallBuilder
{
	static readonly MethodInfo[] _supportedMethods = { Methods.Queryable.Distinct, Methods.Enumerable.Distinct, Methods.DBLayer.SelectDistinct };

	protected override bool CanBuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
	{
		return methodCall.IsSameGenericMethod(_supportedMethods);
	}

	protected override IBuildContext BuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
	{
		var sequence = builder.BuildSequence(new BuildInfo(buildInfo, methodCall.Arguments[0]));
		var sql      = sequence.SelectQuery;

		if (sql.Select.TakeValue != null || sql.Select.SkipValue != null)
			sequence = new SubQueryContext(sequence);

		sequence.SelectQuery.Select.IsDistinct = true;

		// We do not need all fields for SelectDistinct
		//
		if (methodCall.IsSameGenericMethod(Methods.DBLayer.SelectDistinct))
		{
			sequence.SelectQuery.Select.OptimizeDistinct = true;
		}
		else
		{
			sequence.ConvertToIndex(null, 0, ConvertFlags.All);
		}

		return sequence;
	}
}
