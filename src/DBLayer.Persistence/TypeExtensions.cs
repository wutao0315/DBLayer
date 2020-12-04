using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBLayer.Persistence
{
    public static class TypeExtensions
    {
        /// <summary>
        /// 判断type是否为支持async的类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsAsyncType(this Type type)
        {
            var awaiter = type.GetMethod("GetAwaiter");
            if (awaiter == null)
                return false;
            var retType = awaiter.ReturnType;
            //.NET Core 1.1及以下版本中没有 GetInterface 方法，为了兼容性使用 GetInterfaces
            if (retType.GetInterfaces().All(i => i.Name != "INotifyCompletion"))
                return false;
            if (retType.GetProperty("IsCompleted") == null)
                return false;
            if (retType.GetMethod("GetResult") == null)
                return false;

            return true;
        }
        /// <summary>
        /// 获得基础类型，获得比如被Task，ICollection<>，IEnumable<>,IQueryable<>等包裹的类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetUnderlyingType(this Type type)
        {
            var resultTmp = type.IsAsyncType() ? type.GenericTypeArguments.First() : type;
            var resultTmp2 = resultTmp.IsGenericType
                ? resultTmp.GetGenericArguments().First()
                : resultTmp;

            return resultTmp2;
        }
    }
}
