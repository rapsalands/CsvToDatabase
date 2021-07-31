using CsvToDatabaseAbstraction.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace CsvToDatabaseAbstraction.Helpers
{
    /// <summary>
    /// Utility functions for Column
    /// </summary>
    public class ColumnUtility
    {
        /// <summary>
        /// Creates column definitions based on the column names passed. Other values are default.
        /// Use ConfigureDefinitions method to make changes.
        /// </summary>
        /// <param name="columnNames"></param>
        /// <returns></returns>
        public List<ColumnDefinition> CreateDefinitions(IEnumerable<string> columnNames)
        {
            var columnDefinitions = new List<ColumnDefinition>();

            foreach (var columnName in columnNames)
            {
                var cd = new ColumnDefinition(FormatColumnName(columnName))
                {
                    Key = columnName
                };
                columnDefinitions.Add(cd);
            }

            return columnDefinitions;
        }

        /// <summary>
        /// Configure/Modify column definitions based on the records passed.
        /// </summary>
        /// <param name="records">All records of a file.</param>
        /// <param name="columnDefinitions">Default column definitions.</param>
        public void ConfigureDefinitions(List<object> records, List<ColumnDefinition> columnDefinitions)
        {
            foreach (ExpandoObject recordObject in records)
            {
                var record = new Dictionary<string, object>(recordObject);

                foreach (var cd in columnDefinitions)
                {
                    cd.SetMaxColumnType(Determine(record[cd.Key].ToString()));
                    cd.SetIsNull(record[cd.Key].ToString());
                }
            }
        }

        /// <summary>
        /// Determine column type based on the value passed.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private ColumnType Determine(string value)
        {
            var parsed = bool.TryParse(value, out _);
            if (parsed) return ColumnType.Bit;

            parsed = DateTime.TryParse(value, out _);
            if (parsed) return ColumnType.DateTime;

            parsed = long.TryParse(value, out _);
            if (parsed) return ColumnType.Int;

            return ColumnType.VarChar;
        }

        /// <summary>
        /// Format CSV column header value by removing spaces and capitalizing first letter.
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        private string FormatColumnName(string columnName)
        {
            columnName = $"{char.ToUpper(columnName[0])}{columnName.Substring(1)}";
            columnName = columnName.Replace(" ", "");
            return columnName;
        }

        /// <summary>
        /// Returns new primary column with default values and identity as true.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ColumnDefinition DefaultPrimaryColumn(string name)
        {
            return new ColumnDefinition
            {
                Name = name,
                IsPrimary = true,
                Identity = true,
                ColumnType = ColumnType.Int,
                IsNull = false,
            };
        }

        /// <summary>
        /// Checks if definitions passed has primary column.
        /// </summary>
        /// <param name="definitions"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool HasPrimary(IEnumerable<ColumnDefinition> definitions, string name = "Id")
        {
            var primary = definitions.SingleOrDefault(n => n.Name == name && n.IsPrimary);
            return primary != null;
        }
    }
}
