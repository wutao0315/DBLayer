﻿using System.Linq.Expressions;
using DBLayer.Expressions;
using DBLayer.SqlQuery;

using static DBLayer.Reflection.Methods.DBLayer.Merge;

namespace DBLayer.Linq.Builder;

internal partial class MergeBuilder
{
	internal class DeleteWhenNotMatchedBySource : MethodCallBuilder
	{
		protected override bool CanBuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			return methodCall.IsSameGenericMethod(DeleteWhenNotMatchedBySourceAndMethodInfo);
		}

		protected override IBuildContext BuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			var mergeContext = (MergeContext)builder.BuildSequence(new BuildInfo(buildInfo, methodCall.Arguments[0]));

			var statement = mergeContext.Merge;
			var operation = new SqlMergeOperationClause(MergeOperationType.DeleteBySource);
			statement.Operations.Add(operation);

			var predicate = methodCall.Arguments[1];
			if (!predicate.IsNullValue())
			{
				var condition   = (LambdaExpression)predicate.Unwrap();
				operation.Where = BuildSearchCondition(builder, statement, mergeContext.TargetContext, null, condition);
			}

			return mergeContext;
		}
	}
}