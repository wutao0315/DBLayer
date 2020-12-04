using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace DBLayer.Core.Interface
{
    public interface IProxyBuilder
    {
        object Build(Type interfaceType, params object[] constructor);
        T Build<T>(params object[] constructor);
        public class DefaultProxyBuilder : IProxyBuilder
        {
            private Type targetType;
            public object Build(Type interfaceType, params object[] constructor)
            {
                targetType = interfaceType;
                string assemblyName = targetType.Name + "_ProxyAssembly";
                string moduleName = targetType.Name + "_ProxyModule";
                string typeName = targetType.Name + "_Proxy";

                AssemblyName assyName = new AssemblyName(assemblyName);
                AssemblyBuilder assyBuilder = AssemblyBuilder.DefineDynamicAssembly(assyName, AssemblyBuilderAccess.Run);
                ModuleBuilder modBuilder = assyBuilder.DefineDynamicModule(moduleName);

                //新类型的属性
                TypeAttributes newTypeAttribute = TypeAttributes.Class | TypeAttributes.Public;
                //父类型
                Type parentType;
                //要实现的接口
                Type[] interfaceTypes;

                if (targetType.IsInterface)
                {
                    parentType = typeof(object);
                    interfaceTypes = new Type[] { targetType };
                }
                else
                {
                    parentType = targetType;
                    interfaceTypes = Type.EmptyTypes;
                }
                //得到类型生成器            
                TypeBuilder typeBuilder = modBuilder.DefineType(typeName, newTypeAttribute, parentType, interfaceTypes);

                var resultType = typeBuilder.CreateTypeInfo().AsType();
                var result = Activator.CreateInstance(resultType, args: constructor);
                return result;
            }

            public T Build<T>(params object[] constructor)
            {
                return (T)this.Build(typeof(T), constructor);
            }
        }
    }
}
