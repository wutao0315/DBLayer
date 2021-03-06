﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

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
            var pis = tpEntity.GetCachedProperties();
            var a = pis.Keys.FirstOrDefault(m => m.Name == feildname);
            if (a != null)
            {
                object obj = pis[a].Getter(obEntity);
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
            var pis = tpEntity.GetCachedProperties();
            var a = pis.Keys.FirstOrDefault(m => m.Name == feildname);
            if (a != null)
            {
                pis[a].Setter(obEntity, Value);
               //a.SetValue(obEntity, Value.ChangeType(a.PropertyType), null);
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

    public static class ObjectReflectionExtensions
    {
        private static readonly ConcurrentDictionary<Type, Dictionary<PropertyInfo, PropertyGetterSetter>> CachedProperties;
        static ObjectReflectionExtensions()
        {
            CachedProperties = new ConcurrentDictionary<Type, Dictionary<PropertyInfo, PropertyGetterSetter>>();
        }

        public static Dictionary<PropertyInfo, PropertyGetterSetter> GetCachedProperties(this object obj) 
        {
            var targetType = obj.GetType();
            var result = targetType.GetCachedProperties();
            return result;
        }

        public static Dictionary<PropertyInfo, PropertyGetterSetter> GetCachedProperties(this Type type)
        {
            if (CachedProperties.TryGetValue(type, out var properties))
            {
                return properties;
            }

            CacheProperties(type);
            return CachedProperties[type];
        }

        private static void CacheProperties(Type type)
        {
            if (CachedProperties.ContainsKey(type))
            {
                return;
            }

            CachedProperties[type] = new Dictionary<PropertyInfo, PropertyGetterSetter>();
            var properties = type.GetProperties().Where(x => x.PropertyType.IsPublic && x.CanRead && x.CanWrite);
            foreach (var propertyInfo in properties)
            {
                var getter = CompilePropertyGetter(propertyInfo);
                var setter = CompilePropertySetter(propertyInfo);
                var val = new PropertyGetterSetter(getter, setter);
                CachedProperties[type].Add(propertyInfo, val);
                if (!propertyInfo.PropertyType.IsValueTypeOrString())
                {
                    if (propertyInfo.PropertyType.IsIEnumerable())
                    {
                        var types = propertyInfo.PropertyType.GetGenericArguments();
                        foreach (var genericType in types)
                        {
                            if (!genericType.IsValueTypeOrString())
                            {
                                CacheProperties(genericType);
                            }
                        }
                    }
                    else
                    {
                        CacheProperties(propertyInfo.PropertyType);
                    }
                }
            }
        }

        // Inspired by Zanid Haytam
        // https://blog.zhaytam.com/2020/11/17/expression-trees-property-getter/
        private static Func<object, object> CompilePropertyGetter(PropertyInfo property)
        {
            var objectType = typeof(object);
            var objectParameter = Expression.Parameter(objectType);
            var castExpression = Expression.TypeAs(objectParameter, property.DeclaringType);
            var convertExpression = Expression.Convert(
                Expression.Property(castExpression, property),
                objectType);
            return Expression.Lambda<Func<object, object>>(
                convertExpression,
                objectParameter).Compile();
        }

        private static Action<object, object> CompilePropertySetter(PropertyInfo property)
        {
            var objectType = typeof(object);

            var targetParameter = Expression.Parameter(objectType);
            var castExpression = Expression.TypeAs(targetParameter, property.DeclaringType);
            var propertyExpression = Expression.Property(castExpression, property);

            var valueExpression = Expression.Parameter(objectType);

            var changeTypeMethod = typeof(ObjectExtensions).GetMethod("ChangeType");
            var valueConvertExpression = Expression.Convert(
                Expression.Call(changeTypeMethod, valueExpression, Expression.Constant(property.PropertyType)),
                property.PropertyType);

            var result = Expression.Lambda<Action<object, object>>(
                Expression.Assign(propertyExpression, valueConvertExpression),
                targetParameter,
                valueExpression
            ).Compile();

            return result;
        }

        internal static bool IsValueTypeOrString(this Type type)
        {
            return type.IsValueType || type == typeof(string);
        }

        internal static string ToStringValueType(this object value)
        {
            return value switch
            {
                DateTime dateTime => dateTime.ToString("o"),
                bool boolean => boolean.ToStringLowerCase(),
                _ => value.ToString()
            };
        }

        internal static bool IsIEnumerable(this Type type)
        {
            return type.IsAssignableFrom(typeof(IEnumerable));
        }

        internal static string ToStringLowerCase(this bool boolean)
        {
            return boolean ? "true" : "false";
        }
    }
    public class PropertyGetterSetter 
    {
        public PropertyGetterSetter(Func<object, object> getter, Action<object, object> setter) 
        {
            Getter = getter;
            Setter = setter;
        }
        public Func<object, object> Getter { get; }
        public Action<object, object> Setter { get; }
    }
}
