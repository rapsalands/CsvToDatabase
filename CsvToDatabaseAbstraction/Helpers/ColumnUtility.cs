using CsvToDatabaseAbstraction.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace CsvToDatabaseAbstraction.Helpers
{
    public class ColumnUtility
    {
        public ColumnUtility()
        {

        }

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

        private string FormatColumnName(string columnName)
        {
            columnName = $"{char.ToUpper(columnName[0])}{columnName.Substring(1)}";
            columnName = columnName.Replace(" ", "");
            return columnName;
        }

        public ColumnDefinition PrimaryColumn(string name = "Id")
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

        public bool HasPrimary(IEnumerable<ColumnDefinition> columnDefinitions, string name = "Id")
        {
            var primary = columnDefinitions.SingleOrDefault(n => n.Name == name && n.IsPrimary);
            return primary != null;
        }

        public ColumnDefinition GetPrimary(List<ColumnDefinition> definitions, string name = "Id")
        {
            var primary = definitions.SingleOrDefault(n => n.Name == name && n.IsPrimary);
            return primary;
        }
    }
}
