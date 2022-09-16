using DBLayer.Common;

namespace DBLayer.SqlQuery;

public class AliasesContext
{
	readonly HashSet<IQueryElement> _aliasesSet = new (Utils.ObjectReferenceEqualityComparer<IQueryElement>.Default);

	public void RegisterAliased(IQueryElement element)
	{
		_aliasesSet.Add(element);
	}

	public void RegisterAliased(IReadOnlyCollection<IQueryElement> elements)
	{
		_aliasesSet.AddRange(elements);
	}

	public bool IsAliased(IQueryElement element)
	{
		return _aliasesSet.Contains(element);
	}


	public IReadOnlyCollection<IQueryElement> GetAliased()
	{
		return _aliasesSet;
	}

	public HashSet<string> GetUsedTableAliases()
	{
		return new(_aliasesSet.Where(e => e.ElementType == QueryElementType.TableSource)
				.Select(e => ((SqlTableSource)e).Alias!),
			StringComparer.OrdinalIgnoreCase);

	}

	public SqlParameter[] GetParameters()
	{
		return _aliasesSet.Where(e => e.ElementType == QueryElementType.SqlParameter)
			.Select(e => (SqlParameter)e).ToArray();
	}
}
