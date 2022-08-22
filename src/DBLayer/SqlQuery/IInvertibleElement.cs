namespace DBLayer.SqlQuery;

public interface IInvertibleElement
{
	bool CanInvert();
	IQueryElement Invert();
}
