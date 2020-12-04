using System;

namespace DBLayer.Persistence
{
    public static class ObjectExtensions
    {
        public static object ChangeType(this object value, Type type)
        {
            if (value == null && type.IsGenericType) return Activator.CreateInstance(type);
            if (value == null) return null;
            if (type == value.GetType()) return value;
            if (type.IsEnum)
            {
                if (value is string)
                    return Enum.Parse(type, value as string);
                else
                    return Enum.ToObject(type, value);
            }
            if (!type.IsInterface && type.IsGenericType)
            {
                Type innerType = type.GetGenericArguments()[0];
                object innerValue = ChangeType(value, innerType);
                return Activator.CreateInstance(type, new object[] { innerValue });
            }
            if (value is string && type == typeof(Guid)) return new Guid(value as string);
            if (value is string && type == typeof(Version)) return new Version(value as string);
            if (!(value is IConvertible)) return value;

            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                var underlyingType = Nullable.GetUnderlyingType(type);
                type = underlyingType ?? type;
            } // end if
            return Convert.ChangeType(value, type);
        } 
    }
}
