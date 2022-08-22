using System.Reflection;

namespace DBLayer.Linq
{
	using System;
	using System.Linq;
	using Common;
	using DBLayer.Linq.Builder;
	using DBLayer.Mapping;

	internal static class Exceptions
	{
		internal static object DefaultInheritanceMappingException(object value, Type type)
		{
			throw new LinqException("Inheritance mapping is not defined for discriminator value '{0}' in the '{1}' hierarchy.", value, type);
		}
	}
}
