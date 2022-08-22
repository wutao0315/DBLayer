using System.Collections.Generic;
using System.Text;

namespace DBLayer.SqlQuery
{
	public interface IQueryElement
	{
		QueryElementType ElementType { get; }
		StringBuilder    ToString (StringBuilder sb, Dictionary<IQueryElement,IQueryElement> dic);
	}
}
