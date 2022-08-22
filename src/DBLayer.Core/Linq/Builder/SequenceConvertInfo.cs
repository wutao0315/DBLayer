using System.Collections.Generic;
using System.Linq.Expressions;

namespace DBLayer.Linq.Builder
{
	public class SequenceConvertInfo
	{
		public ParameterExpression?       Parameter;
		public Expression                 Expression = null!;
		public List<SequenceConvertPath>? ExpressionsToReplace;
	}
}
