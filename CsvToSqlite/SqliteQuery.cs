using CsvToDatabaseAbstraction;
using CsvToDatabaseAbstraction.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsvToSqlite
{
    /// <summary>
    /// SQLite based query.
    /// </summary>
    public class SqliteQuery : DbQuery
    {
        /// <summary>
        /// SQLite based query.
        /// </summary>
        /// <param name="databasePath">Database path. Path should exclude database extension.</param>
        public SqliteQuery(string databasePath) : base(databasePath)
        {

        }

        /// <summary>
        /// SQL query for creating a table without data.
        /// </summary>
        /// <param name="tableOption">Table Option</param>
        /// <returns></returns>
        public override string CreateTable(TableOption tableOption)
        {
            var tableName = tableOption.Name;
            string sql = $@"create table {tableName} ({ColumnsQuery(tableOption)})";
            return sql;
        }

        /// <summary>
        /// SQL query for inserting data in table row. This query will be executed for each row.
        /// CsvToDatabase is designed to reuse command object improve performance.
        /// </summary>
        /// <param name="tableOption">Table Option</param>
        /// <param name="columnParameterMappings">Column Paramater Mapping</param>
        /// <returns></returns>
        public override string Insert(TableOption tableOption, List<ColumnParameterMapping> columnParameterMappings)
        {
            var colNames = columnParameterMappings.Select(n => n.ColumnName).ToArray();
            var paramNames = columnParameterMappings.Select(n => n.ParameterKey).ToArray();

            string sql = $@"insert into {tableOption.Name} 
                                ({string.Join(',', colNames)})
                                values ({string.Join(',', paramNames)})";

            return sql;
        }

        /// <summary>
        /// Returns query for adding columns to database table.
        /// </summary>
        /// <param name="tableOption">Table Option</param>
        /// <returns></returns>
        string ColumnsQuery(TableOption tableOption)
        {
            var sb = new StringBuilder();

            foreach (var cd in tableOption.ColumnDefinitions)
            {
                if (sb.Length > 0) sb.Append(",");
                sb.Append($"{cd.Name} {ColumnTypeText(cd)} {NullText(cd)}");
            }

            if (tableOption.HasPrimary)
            {
                sb.Append(",");

                var autoincrement = tableOption.Primary.Identity ? "AUTOINCREMENT" : "";
                sb.Append($"PRIMARY KEY ({tableOption.Primary.Name} {autoincrement})");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Return column type based on column definition.
        /// </summary>
        /// <param name="columnDefinition"></param>
        /// <returns></returns>
        string ColumnTypeText(ColumnDefinition columnDefinition)
        {
            string integer = "INTEGER", text = "TEXT";

            switch (columnDefinition.ColumnType)
            {
                case ColumnType.Bit:
                case ColumnType.Int:
                    return integer;

                case ColumnType.DateTime:
                case ColumnType.VarChar:
                    return $"{text}({columnDefinition.MaxLength})";
                default:
                    return text;
            }
        }

        /// <summary>
        /// Returns text for Null/NotNull based on argument.
        /// </summary>
        /// <param name="columnDefinition"></param>
        /// <returns></returns>
        string NullText(ColumnDefinition columnDefinition) => columnDefinition.IsNull ? "NULL" : "NOT NULL";
    }
}
