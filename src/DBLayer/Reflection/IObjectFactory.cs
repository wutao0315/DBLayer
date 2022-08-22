namespace DBLayer.Reflection;

public interface IObjectFactory
{
	object CreateInstance(TypeAccessor typeAccessor);
}
