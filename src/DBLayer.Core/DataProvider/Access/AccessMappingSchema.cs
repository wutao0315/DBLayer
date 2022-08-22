﻿using DBLayer.Common;
using DBLayer.Mapping;
using DBLayer.SqlQuery;
using System.Data.Linq;
using System.Globalization;
using System.Text;

namespace DBLayer.DataProvider.Access;

sealed class AccessMappingSchema : LockedMappingSchema
{
	private const string DATE_FORMAT     = "#{0:yyyy-MM-dd}#";
	private const string DATETIME_FORMAT = "#{0:yyyy-MM-dd HH:mm:ss}#";

	AccessMappingSchema() : base(ProviderName.Access)
	{
		SetDataType(typeof(DateTime),  DataType.DateTime);
		SetDataType(typeof(DateTime?), DataType.DateTime);

		SetValueToSqlConverter(typeof(bool),     (sb,dt,v) => sb.Append(v));
		SetValueToSqlConverter(typeof(Guid),     (sb,dt,v) => sb.Append('\'').Append(((Guid)v).ToString("B")).Append('\''));
		SetValueToSqlConverter(typeof(DateTime), (sb,dt,v) => ConvertDateTimeToSql(sb, (DateTime)v));
		SetValueToSqlConverter(typeof(DateOnly), (sb,dt,v) => ConvertDateOnlyToSql(sb, (DateOnly)v));

		SetDataType(typeof(string), new SqlDataType(DataType.NVarChar, typeof(string), 255));

		SetValueToSqlConverter(typeof(string),   (sb,dt,v) => ConvertStringToSql  (sb, v.ToString()!));
		SetValueToSqlConverter(typeof(char),     (sb,dt,v) => ConvertCharToSql    (sb, (char)v));
		SetValueToSqlConverter(typeof(byte[]),   (sb,dt,v) => ConvertBinaryToSql  (sb, (byte[])v));
		SetValueToSqlConverter(typeof(Binary),   (sb,dt,v) => ConvertBinaryToSql  (sb, ((Binary)v).ToArray()));

		// Why:
		// 1. Access use culture-specific string format for decimals
		// 2. we need to use string type for decimal parameters
		// This leads to issues with database data parsing as ConvertBuilder will generate parse with InvariantCulture
		// We need to specify culture-specific converter explicitly
		SetConvertExpression((string v) => decimal.Parse(v));
		SetConvertExpression((string v) => float.Parse(v));
		SetConvertExpression((string v) => double.Parse(v));
	}

	static void ConvertBinaryToSql(StringBuilder stringBuilder, byte[] value)
	{
		stringBuilder
			.Append("0x")
			.AppendByteArrayAsHexViaLookup32(value);
	}

	static readonly Action<StringBuilder, int> _appendConversionAction = AppendConversion;

	static void AppendConversion(StringBuilder stringBuilder, int value)
	{
		stringBuilder
			.Append("chr(")
			.Append(value)
			.Append(')')
			;
	}

	static void ConvertStringToSql(StringBuilder stringBuilder, string value)
	{
		DataTools.ConvertStringToSql(stringBuilder, "+", null, _appendConversionAction, value, null);
	}

	static void ConvertCharToSql(StringBuilder stringBuilder, char value)
	{
		DataTools.ConvertCharToSql(stringBuilder, "'", _appendConversionAction, value);
	}

	static void ConvertDateTimeToSql(StringBuilder stringBuilder, DateTime value)
	{
		var format = value.Hour == 0 && value.Minute == 0 && value.Second == 0 ? DATE_FORMAT : DATETIME_FORMAT;

		stringBuilder.AppendFormat(CultureInfo.InvariantCulture, format, value);
	}

	static void ConvertDateOnlyToSql(StringBuilder stringBuilder, DateOnly value)
	{
		stringBuilder.AppendFormat(CultureInfo.InvariantCulture, DATE_FORMAT, value);
	}

	internal static readonly AccessMappingSchema Instance = new ();

	public sealed class OleDbMappingSchema : LockedMappingSchema
	{
		public OleDbMappingSchema() : base(ProviderName.Access, Instance)
		{
		}
	}

	public sealed class OdbcMappingSchema : LockedMappingSchema
	{
		public OdbcMappingSchema() : base(ProviderName.AccessOdbc, Instance)
		{
		}
	}
}
