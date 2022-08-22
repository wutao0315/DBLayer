using System.Linq.Expressions;
using DBLayer.Common;
using DBLayer.Mapping;

namespace DBLayer.Expressions;

public class DefaultValueExpression : Expression
{
	public DefaultValueExpression(MappingSchema? mappingSchema, Type type)
	{
		_mappingSchema = mappingSchema;
		_type          = type;
	}

	readonly MappingSchema? _mappingSchema;
	readonly Type           _type;

	public override Type           Type      => _type;
	public override ExpressionType NodeType  => ExpressionType.Extension;
	public override bool           CanReduce => true;

	public override Expression Reduce()
	{
		return Constant(
			_mappingSchema == null ?
				DefaultValue.GetValue(Type) :
				_mappingSchema.GetDefaultValue(Type),
			Type);
	}

	public override string ToString()
	{
		return $"Default({Type.Name})";
	}
}
