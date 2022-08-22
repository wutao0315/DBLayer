﻿using DBLayer.Common;
using DBLayer.Mapping;
using DBLayer.SqlQuery;
using System.Reflection;
using System.Text;

namespace DBLayer.Extensions;

static class MappingExtensions
{
	public static SqlValue GetSqlValueFromObject(this MappingSchema mappingSchema, ColumnDescriptor columnDescriptor, object obj)
	{
		var providerValue = columnDescriptor.GetProviderValue(obj);

		return new SqlValue(columnDescriptor.GetDbDataType(true), providerValue);
	}

	public static SqlValue GetSqlValue(this MappingSchema mappingSchema, Type systemType, object? originalValue)
	{
		var underlyingType = systemType.ToNullableUnderlying();

		if (!mappingSchema.ValueToSqlConverter.CanConvert(underlyingType))
		{
			if (underlyingType.IsEnum && mappingSchema.GetAttribute<Sql.EnumAttribute>(underlyingType) == null)
			{
				if (originalValue != null || systemType == underlyingType)
				{
					var type = Converter.GetDefaultMappingFromEnumType(mappingSchema, systemType)!;

					if (Configuration.UseEnumValueNameForStringColumns && type == typeof(string) &&
					    mappingSchema.GetMapValues(underlyingType)             == null)
						return new SqlValue(type, originalValue!.ToString());

					return new SqlValue(type, Converter.ChangeType(originalValue, type, mappingSchema));
				}
			}
		}

		if (systemType == typeof(object) && originalValue != null)
			systemType = originalValue.GetType();

		return new SqlValue(systemType, originalValue);
	}

	public static bool TryConvertToSql(this MappingSchema mappingSchema, StringBuilder stringBuilder, SqlDataType? dataType, object? value)
	{
		var sqlConverter = mappingSchema.ValueToSqlConverter;

		if (value is null)
		{
			return sqlConverter.TryConvert(stringBuilder, dataType, value);
		}

		var systemType     = value.GetType();
		var underlyingType = systemType.ToNullableUnderlying();

		if (!mappingSchema.ValueToSqlConverter.CanConvert(underlyingType))
		{
			if (underlyingType.IsEnum && mappingSchema.GetAttribute<Sql.EnumAttribute>(underlyingType) == null)
			{
				if (systemType == underlyingType)
				{
					var type = Converter.GetDefaultMappingFromEnumType(mappingSchema, systemType)!;

					if (Configuration.UseEnumValueNameForStringColumns && type == typeof(string) &&
					    mappingSchema.GetMapValues(underlyingType)             == null)
						value = value.ToString();
					else
						value = Converter.ChangeType(value, type, mappingSchema);
				}
			}
		}

		return sqlConverter.TryConvert(stringBuilder, dataType, value);
	}

	public static void ConvertToSqlValue(this MappingSchema mappingSchema, StringBuilder stringBuilder,
		SqlDataType? dataType, object? value)
	{
		if (!mappingSchema.TryConvertToSql(stringBuilder, dataType, value))
			throw new DBLayerException($"Cannot convert value of type {value?.GetType()} to SQL");
	}

	public static Sql.ExpressionAttribute? GetExpressionAttribute(this MemberInfo member, MappingSchema mappingSchema)
	{
		return mappingSchema.GetAttribute<Sql.ExpressionAttribute>(member.ReflectedType!, member, static a => a.Configuration);
	}

	public static Sql.TableFunctionAttribute? GetTableFunctionAttribute(this MemberInfo member, MappingSchema mappingSchema)
	{
		return mappingSchema.GetAttribute<Sql.TableFunctionAttribute>(member.ReflectedType!, member, static a => a.Configuration);
	}
}