using System.Linq.Expressions;
using DBLayer.Common;
using DBLayer.Expressions;
using DBLayer.SqlQuery;

namespace DBLayer.Linq.Builder;

class WithTableExpressionBuilder : MethodCallBuilder
{
	private static readonly string[] MethodNames =
	{
		nameof(LinqExtensions.With),
		nameof(LinqExtensions.WithTableExpression)
	};

	protected override bool CanBuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
	{
		return methodCall.IsQueryable(MethodNames);
	}

	protected override IBuildContext BuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
	{
		var sequence = builder.BuildSequence(new BuildInfo(buildInfo, methodCall.Arguments[0]));
		var table    = SequenceHelper.GetTableContext(sequence) ?? throw new DBLayerException($"Cannot get table context from {sequence.GetType()}");
		var value    = (string)methodCall.Arguments[1].EvaluateExpression()!;

		table.SqlTable.SqlTableType   = SqlTableType.Expression;
		table.SqlTable.TableArguments = Array<ISqlExpression>.Empty;

		switch (methodCall.Method.Name)
		{
			case nameof(LinqExtensions.With)                : table.SqlTable.Expression = $"{{0}} {{1}} WITH ({value})"; break;
			case nameof(LinqExtensions.WithTableExpression) : table.SqlTable.Expression = value;                         break;
		}

		return sequence;
	}
}
