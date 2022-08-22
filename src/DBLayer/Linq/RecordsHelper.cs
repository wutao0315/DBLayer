using DBLayer.Extensions;
using DBLayer.Mapping;
using System.Collections.Concurrent;
using System.Reflection;

namespace DBLayer.Linq;

[Flags]
internal enum RecordType
{
	/// <summary>
	/// Type is not recognized as record type.
	/// </summary>
	NotRecord     = 0x00,
	/// <summary>
	/// Type is F# record type (has reflection information about members position).
	/// </summary>
	FSharp        = 0x01,
	/// <summary>
	/// Type is C# record class or any other class with constructor parameter mathing properties by name.
	/// </summary>
	RecordClass   = 0x02,
	/// <summary>
	/// Type is C# or VB.NET anonymous type.
	/// </summary>
	AnonymousType = 0x04,

	/// <summary>
	/// Mask for types that instantiated using record-like constructor.
	/// </summary>
	CallConstructorOnWrite = FSharp | RecordClass | AnonymousType,
	/// <summary>
	/// Mask for types that instantiated in expressions using record-like constructor.
	/// </summary>
	CallConstructorOnRead  = FSharp | RecordClass,
}

internal static class RecordsHelper
{
	private static readonly ConcurrentDictionary<Type, RecordType> _recordCache = new ();
	private static readonly ConcurrentDictionary<MemberInfo, int>  _fsharpRecordMemberCache = new ();

	internal static RecordType GetRecordType(MappingSchema mappingSchema, Type objectType)
	{
		return _recordCache.GetOrAdd(objectType, static (objectType, mappingSchema) =>
		{
			if (IsFSharpRecord(mappingSchema, objectType))
				return RecordType.FSharp;

			if (objectType.IsAnonymous())
				return RecordType.AnonymousType;

			if (!HasDefaultConstructor(objectType))
				return RecordType.RecordClass;

			return RecordType.NotRecord;

		}, mappingSchema);
	}

	public static int GetFSharpRecordMemberSequence(MappingSchema mappingSchema, Type objectType, MemberInfo memberInfo)
	{

		return _fsharpRecordMemberCache.GetOrAdd(memberInfo, static (memberInfo, ctx) =>
		{
			var (mappingSchema, objectType) = ctx;
			var attrs                  = mappingSchema.GetAttributes<Attribute>(objectType, memberInfo);
			var compilationMappingAttr = attrs.FirstOrDefault(attr => attr.GetType().FullName == "Microsoft.FSharp.Core.CompilationMappingAttribute");

			if (compilationMappingAttr != null)
			{
				// https://github.com/dotnet/fsharp/blob/1fcb351bb98fe361c7e70172ea51b5e6a4b52ee0/src/fsharp/FSharp.Core/prim-types.fsi
				// ObjectType = 3
				if (Convert.ToInt32(((dynamic)compilationMappingAttr).SourceConstructFlags) != 3)
					return ((dynamic)compilationMappingAttr).SequenceNumber;
			}

			return -1;
		}, (mappingSchema, objectType));
	}

	private static bool IsFSharpRecord(MappingSchema mappingSchema, Type objectType)
	{
		var attrs = mappingSchema.GetAttributes<Attribute>(objectType);

		var compilationMappingAttr = attrs.FirstOrDefault(attr => attr.GetType().FullName == "Microsoft.FSharp.Core.CompilationMappingAttribute");
		if (compilationMappingAttr == null)
			return false;

		// https://github.com/dotnet/fsharp/blob/1fcb351bb98fe361c7e70172ea51b5e6a4b52ee0/src/fsharp/FSharp.Core/prim-types.fsi
		// ObjectType = 3
		if (Convert.ToInt32(((dynamic)compilationMappingAttr).SourceConstructFlags) == 3)
			return false;

		return !attrs.Any(attr => attr.GetType().FullName == "Microsoft.FSharp.Core.CLIMutableAttribute");
	}

	private static bool HasDefaultConstructor(Type objectType)
	{
		var constructors = objectType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

		foreach (var constructor in constructors)
		{
			if (constructor.GetParameters().Length == 0)
				return true;
		}

		return constructors.Length == 0;
	}
}
