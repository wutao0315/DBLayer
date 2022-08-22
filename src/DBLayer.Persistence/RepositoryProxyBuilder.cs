using DBLayer.Interface;
using System.Collections.Concurrent;
using System.Reflection;
using System.Reflection.Emit;

namespace DBLayer.Persistence;

public class RepositoryProxyBuilder : IRepositoyProxyBuilder
{
    private static ConcurrentDictionary<string, Type> TargetTypeCache { set; get; } = new ConcurrentDictionary<string, Type>();

    public object Build(Type interfaceType, params object[] constructor)
    {
        var cacheKey = interfaceType.FullName;
        var resultType = TargetTypeCache.GetOrAdd(cacheKey, (s) => BuildTargetType(interfaceType, constructor));
        var result = Activator.CreateInstance(resultType, args: constructor);
        return result;
    }

    /// <summary>
    /// 动态生成接口的实现类
    /// </summary>
    /// <param name="interfaceType"></param>
    /// <param name="constructor"></param>
    /// <returns></returns>
    private Type BuildTargetType(Type targetType, params object[] constructor)
    {

        if (!targetType.IsInterface)
        {
            throw new Exception($"{targetType.FullName} is not a interface");
        }

        string assemblyName = targetType.Name + "_ProxyAssembly";
        string moduleName = targetType.Name + "_ProxyModule";
        string typeName = targetType.Name + "_Proxy";

        AssemblyName assyName = new AssemblyName(assemblyName);
        AssemblyBuilder assyBuilder = AssemblyBuilder.DefineDynamicAssembly(assyName, AssemblyBuilderAccess.Run);
        ModuleBuilder modBuilder = assyBuilder.DefineDynamicModule(moduleName);

        //新类型的属性
        TypeAttributes newTypeAttribute = TypeAttributes.Class | TypeAttributes.Public;
        //父类型
        Type parentType = null;
        //要实现的接口
        Type[] interfaceTypes = new Type[] { targetType };

        var allInterfaces = targetType.GetInterfaces();

        //优先实例化泛型
        foreach (var iInterface in allInterfaces)
        {
            if (iInterface.IsGenericType)
            {
                var isRepository = typeof(IRepository<,>).IsAssignableFrom(iInterface.GetGenericTypeDefinition());
                if (isRepository)
                {
                    var genericType = iInterface.GetGenericArguments();
                    parentType = typeof(BaseRepository<,>).MakeGenericType(genericType);
                    break;
                }
            }
            else
            {
                var isRepository = typeof(IRepository).IsAssignableFrom(iInterface);
                if (isRepository)
                {
                    parentType = typeof(BaseRepository);
                }
            }
        }

        if (parentType == null)
        {
            throw new Exception($"{targetType.FullName} please assignable from {typeof(IRepository<,>).FullName} or {typeof(IRepository).FullName}");
        }

        //得到类型生成器            
        TypeBuilder typeBuilder = modBuilder.DefineType(typeName, newTypeAttribute, parentType, interfaceTypes);

        var cotrParameterTypes = new Type[] { typeof(IDbContext) };
        //创建构造函数
        ConstructorBuilder constructorBuilder =
            typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, cotrParameterTypes);

        var baseCtor = parentType.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, cotrParameterTypes, null);

        //il创建构造函数，对httpService和IServiceProvider两个字段进行赋值，同时初始化存放参数的集合
        ILGenerator ilgCtor = constructorBuilder.GetILGenerator();

        ilgCtor.Emit(OpCodes.Ldarg_0); //加载当前类
        ilgCtor.Emit(OpCodes.Ldarg_1);
        //ilgCtor.Emit(OpCodes.Ldarg_2);
        ilgCtor.Emit(OpCodes.Call, baseCtor);

        ilgCtor.Emit(OpCodes.Nop);
        ilgCtor.Emit(OpCodes.Nop);
        ilgCtor.Emit(OpCodes.Ret); //返回

        var resultType = typeBuilder.CreateTypeInfo().AsType();
        return resultType;
    }

    public T Build<T>(params object[] constructor)
    {
        return (T)this.Build(typeof(T), constructor);
    }
}
