﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DBLayer.DataProvider.SapHana
{
	using SqlQuery;
	using SqlProvider;
	using Mapping;

	partial class SapHanaSqlBuilder : BasicSqlBuilder
	{
		public SapHanaSqlBuilder(IDataProvider? provider, MappingSchema mappingSchema, ISqlOptimizer sqlOptimizer, SqlProviderFlags sqlProviderFlags)
			: base(provider, mappingSchema, sqlOptimizer, sqlProviderFlags)
		{
		}

		protected SapHanaSqlBuilder(BasicSqlBuilder parentBuilder) : base(parentBuilder)
		{
		}

		protected override ISqlBuilder CreateSqlBuilder()
		{
			return new SapHanaSqlBuilder(this);
		}

		public override int CommandCount(SqlStatement statement)
		{
			return statement.NeedsIdentity() ? 2 : 1;
		}

		protected override void BuildCommand(SqlStatement statement, int commandNumber)
		{
			var insertClause = Statement.GetInsertClause();
			if (insertClause != null)
			{
				var identityField = insertClause.Into!.GetIdentityField();
				var table = insertClause.Into;

				if (identityField == null || table == null)
					throw new SqlException("Identity field must be defined for '{0}'.", insertClause.Into.NameForLogging);

				StringBuilder.Append("SELECT CURRENT_IDENTITY_VALUE() FROM DUMMY");
			}
		}

		protected override string LimitFormat(SelectQuery selectQuery)
		{
			return "LIMIT {0}";
		}

		protected override string OffsetFormat(SelectQuery selectQuery)
		{
			return selectQuery.Select.TakeValue == null ? "LIMIT 4200000000 OFFSET {0}" : "OFFSET {0}";
		}

		public override bool IsNestedJoinParenthesisRequired => true;

		protected override void BuildStartCreateTableStatement(SqlCreateTableStatement createTable)
		{
			if (createTable.StatementHeader == null)
			{
				AppendIndent().Append("CREATE COLUMN TABLE ");
				BuildPhysicalTable(createTable.Table!, null);
			}
			else
			{
				var name = WithStringBuilder(
					new StringBuilder(),
					() =>
					{
						BuildPhysicalTable(createTable.Table!, null);
						return StringBuilder.ToString();
					});

				AppendIndent().AppendFormat(createTable.StatementHeader, name);
			}
		}

		protected override void BuildDataTypeFromDataType(SqlDataType type, bool forCreateTable, bool canBeNull)
		{
			switch (type.Type.DataType)
			{
				case DataType.Int32         :
				case DataType.UInt16        :
					StringBuilder.Append("Integer");
					return;
				case DataType.Double:
					StringBuilder.Append("Double");
					return;
				case DataType.DateTime2     :
				case DataType.DateTime      :
				case DataType.Time:
					StringBuilder.Append("Timestamp");
					return;
				case DataType.SmallDateTime :
					StringBuilder.Append("SecondDate");
					return;
				case DataType.Boolean       :
					StringBuilder.Append("TinyInt");
					return;
				case DataType.Image:
					StringBuilder.Append("Blob");
					return;
				case DataType.Xml:
					StringBuilder.Append("Clob");
					return;
				case DataType.Guid:
					StringBuilder.Append("Char (36)");
					return;
				case DataType.NVarChar:
				case DataType.VarChar:
				case DataType.VarBinary:
					if (type.Type.Length == null || type.Type.Length > 5000 || type.Type.Length < 1)
					{
						StringBuilder
							.Append(type.Type.DataType)
							.Append("(5000)");
						return;
					}
					break;
			}
			base.BuildDataTypeFromDataType(type, forCreateTable, canBeNull);
		}

		protected override void BuildFromClause(SqlStatement statement, SelectQuery selectQuery)
		{
			if (selectQuery.From.Tables.Count == 0)
				StringBuilder.Append("FROM DUMMY").AppendLine();
			else
				base.BuildFromClause(statement, selectQuery);
		}

		public override StringBuilder Convert(StringBuilder sb, string value, ConvertType convertType)
		{
			switch (convertType)
			{
				case ConvertType.NameToQueryParameter:
					return sb.Append(':')
						.Append('"').Append(value).Append('"');

				case ConvertType.NameToCommandParameter:
				case ConvertType.NameToSprocParameter:
				case ConvertType.SprocParameterToName:
					return sb.Append(value);

				case ConvertType.NameToQueryField     :
				case ConvertType.NameToQueryFieldAlias:
				case ConvertType.NameToQueryTableAlias:
					{
						if (value.Length > 0 && value[0] == '"')
							return sb.Append(value);
						return sb.Append('"').Append(value).Append('"');
					}

				case ConvertType.NameToServer    :
				case ConvertType.NameToDatabase  :
				case ConvertType.NameToSchema    :
				case ConvertType.NameToPackage   :
				case ConvertType.NameToQueryTable:
				case ConvertType.NameToProcedure :
					if (value.Length > 0 && value[0] == '\"')
						return sb.Append(value);

					return sb.Append('"').Append(value).Append('"');
			}

			return sb.Append(value);
		}

		protected override void BuildCreateTableIdentityAttribute1(SqlField field)
		{
			StringBuilder.Append("GENERATED BY DEFAULT AS IDENTITY");
		}

		protected override void BuildCreateTablePrimaryKey(SqlCreateTableStatement createTable, string pkName, IEnumerable<string> fieldNames)
		{
			AppendIndent();
			StringBuilder.Append("PRIMARY KEY (");
			StringBuilder.Append(string.Join(InlineComma, fieldNames));
			StringBuilder.Append(')');
		}

		public override StringBuilder BuildObjectName(StringBuilder sb, SqlObjectName name, ConvertType objectType, bool escape, TableOptions tableOptions)
		{
			// <table_name> ::= [[<linked_server_name>.]<schema_name>.][library_name:]<identifier>
			if (name.Server != null && name.Schema == null)
				throw new DBLayerException("You must specify schema name for linked server queries.");

			if (name.Server != null)
			{
				(escape ? Convert(sb, name.Server, ConvertType.NameToServer) : sb.Append(name.Server))
					.Append('.');
			}

			if (name.Schema != null)
			{
				(escape ? Convert(sb, name.Schema, ConvertType.NameToSchema) : sb.Append(name.Schema))
					.Append('.');
			}

			if (name.Package != null)
			{
				(escape ? Convert(sb, name.Package, ConvertType.NameToPackage) : sb.Append(name.Package))
					.Append(':');
			}

			return escape ? Convert(sb, name.Name, objectType) : sb.Append(name.Name);
		}

		protected override void BuildCreateTableCommand(SqlTable table)
		{
			string command;

			if (table.TableOptions.IsTemporaryOptionSet())
			{
				switch (table.TableOptions & TableOptions.IsTemporaryOptionSet)
				{
					case TableOptions.IsTemporary                                                                              :
					case TableOptions.IsTemporary |                                          TableOptions.IsLocalTemporaryData :
					case TableOptions.IsTemporary | TableOptions.IsLocalTemporaryStructure                                     :
					case TableOptions.IsTemporary | TableOptions.IsLocalTemporaryStructure | TableOptions.IsLocalTemporaryData :
					case                                                                     TableOptions.IsLocalTemporaryData :
					case                            TableOptions.IsLocalTemporaryStructure                                     :
					case                            TableOptions.IsLocalTemporaryStructure | TableOptions.IsLocalTemporaryData :
						command = "CREATE LOCAL TEMPORARY TABLE ";
						break;
					case TableOptions.IsGlobalTemporaryStructure                                                               :
					case TableOptions.IsGlobalTemporaryStructure | TableOptions.IsLocalTemporaryData                           :
						command = "CREATE GLOBAL TEMPORARY TABLE ";
						break;
					case var value :
						throw new InvalidOperationException($"Incompatible table options '{value}'");
				}
			}
			else
			{
				command = "CREATE TABLE ";
			}

			StringBuilder.Append(command);
		}

		protected override void BuildIsDistinctPredicate(SqlPredicate.IsDistinct expr) => BuildIsDistinctPredicateFallback(expr);
	}
}
