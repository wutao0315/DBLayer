﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace DBLayer.SqlQuery
{
	/// <summary>
	/// Contains names and create helpers for pseudo-functions, generated by linq2db and then converted to database-specific SQL by provider-specific SQL optimizer.
	/// </summary>
	public static class PseudoFunctions
	{
		/// <summary>
		/// Function to convert text parameter to lowercased form: <c>TO_LOWER(string)</c>
		/// </summary>
		public const string TO_LOWER = "$ToLower$";
		public static SqlFunction MakeToLower(ISqlExpression value)
		{
			return new SqlFunction(typeof(string), TO_LOWER, false, true, value)
			{
				CanBeNull = value.CanBeNull
			};
		}

		/// <summary>
		/// Function to convert text parameter to uppercased form: <c>TO_UPPER(string)</c>
		/// </summary>
		public const string TO_UPPER = "$ToUpper$";
		public static SqlFunction MakeToUpper(ISqlExpression value)
		{
			return new SqlFunction(typeof(string), TO_UPPER, false, true, value)
			{
				CanBeNull = value.CanBeNull
			};
		}

		/// <summary>
		/// Function to convert value from one type to another: <c>CONVERT(to_type, from_type, value) { CanBeNull = value.CanBeNull }</c>
		/// </summary>
		public const string CONVERT = "$Convert$";
		public static SqlFunction MakeConvert(SqlDataType toType, SqlDataType fromType, ISqlExpression value)
		{
			return new SqlFunction(toType.SystemType, CONVERT, false, true, toType, fromType, value)
			{
				CanBeNull = value.CanBeNull
			};
		}

		/// <summary>
		/// Function to convert value from one type to another: <c>TRY_CONVERT(to_type, from_type, value) { CanBeNull = true }</c>.
		/// Returns NULL on conversion failure.
		/// </summary>
		public const string TRY_CONVERT = "$TryConvert$";
		public static SqlFunction MakeTryConvert(SqlDataType toType, SqlDataType fromType, ISqlExpression value)
		{
			return new SqlFunction(toType.SystemType, TRY_CONVERT, false, true, toType, fromType, value)
			{
				CanBeNull = true
			};
		}

		/// <summary>
		/// Function to convert value from one type to another: <c>TRY_CONVERT_OR_DEFAULT(to_type, from_type, value, defaultValue) { CanBeNull = value.CanBeNull || defaultValue.CanBeNull }</c>.
		/// Returns provided default value on conversion failure.
		/// </summary>
		public const string TRY_CONVERT_OR_DEFAULT = "$TryConvertOrDefault$";
		public static SqlFunction MakeTryConvertOrDefault(SqlDataType toType, SqlDataType fromType, ISqlExpression value, ISqlExpression defaultValue)
		{
			return new SqlFunction(toType.SystemType, TRY_CONVERT_OR_DEFAULT, false, true, toType, fromType, value, defaultValue)
			{
				CanBeNull = value.CanBeNull || defaultValue.CanBeNull
			};
		}

		/// <summary>
		/// Function to replace one text fragment with another in string: <c>REPLACE(value, oldSubstring, newSubstring)</c>
		/// </summary>
		public const string REPLACE = "$Replace$";
		public static SqlFunction MakeReplace(ISqlExpression value, ISqlExpression oldSubstring, ISqlExpression newSubstring)
		{
			return new SqlFunction(typeof(string), REPLACE, false, true, value, oldSubstring, newSubstring)
			{
				CanBeNull = value.CanBeNull || oldSubstring.CanBeNull || newSubstring.CanBeNull
			};
		}

		/// <summary>
		/// Function to suppress conversion SQL generation for provided value: <c>REMOVE_CONVERT(value, resultType)</c>
		/// </summary>
		public const string REMOVE_CONVERT = "$Convert_Remover$";
		public static SqlFunction MakeRemoveConvert(ISqlExpression value, SqlDataType resultType)
		{
			return new SqlFunction(resultType.SystemType, REMOVE_CONVERT, false, true, value, resultType)
			{
				CanBeNull = value.CanBeNull
			};
		}

		/// <summary>
		/// Function to return first non-null argument: <c>COALESCE(values...)</c>
		/// </summary>
		public const string COALESCE = "$Coalesce$";
		public static SqlFunction MakeCoalesce(Type systemType, params ISqlExpression[] values)
		{
			var canBeNull = true;

			foreach (var value in values)
				if (!value.CanBeNull)
					canBeNull = false;

			return new SqlFunction(systemType, COALESCE, false, true, values)
			{
				CanBeNull = canBeNull
			};
		}
	}
}
