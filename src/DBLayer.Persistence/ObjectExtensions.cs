using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

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
        /// <summary>
        /// 利用反射根据对象和属性名取对应属性的值
        /// </summary>
        /// <param name="feildname"></param>
        /// <param name="obEntity"></param>
        /// <returns></returns>
        public static object GetValueByPropertyName(this object obEntity, string feildname)
        {
            var tpEntity = obEntity.GetType();
            var pis = tpEntity.GetProperties();
            var a = pis.FirstOrDefault(m => m.Name == feildname);
            if (a != null)
            {
                object obj = a.GetValue(obEntity, null);
                return obj;
            }
            return default;
        }
        /// <summary>
        /// 利用反射根据对象和属性名为对应的属性赋值
        /// </summary>
        /// <param name="feildname"></param>
        /// <param name="obEntity"></param>
        /// <returns></returns>
        public static void SetValueByPropertyName(this object obEntity, string feildname, object Value)
        {
            var tpEntity = obEntity.GetType();
            var pis = tpEntity.GetProperties();
            var a = pis.FirstOrDefault(m => m.Name == feildname);
            if (a != null)
            {
                a.SetValue(obEntity, Value.ChangeType(a.PropertyType), null);
            }
        }
        /// <summary>
        /// 判断指定的类型 <paramref name="type"/> 是否是指定泛型类型的子类型，或实现了指定泛型接口。
        /// </summary>
        /// <param name="type">需要测试的类型。</param>
        /// <param name="generic">泛型接口类型，传入 typeof(IXxx&lt;&gt;)</param>
        /// <returns>如果是泛型接口的子类型，则返回 true，否则返回 false。</returns>
        public static bool HasImplementedRawGeneric([NotNull] this Type type, [NotNull] Type generic)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (generic == null) throw new ArgumentNullException(nameof(generic));

            // 测试接口。
            var isTheRawGenericType = type.GetInterfaces().Any(IsTheRawGenericType);
            if (isTheRawGenericType) return true;

            // 测试类型。
            while (type != null && type != typeof(object))
            {
                isTheRawGenericType = IsTheRawGenericType(type);
                if (isTheRawGenericType) return true;
                type = type.BaseType;
            }

            // 没有找到任何匹配的接口或类型。
            return false;

            // 测试某个类型是否是指定的原始接口。
            bool IsTheRawGenericType(Type test)
                => generic == (test.IsGenericType ? test.GetGenericTypeDefinition() : test);
        }
    }
}
