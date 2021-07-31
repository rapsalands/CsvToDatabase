using System.Data.Common;

namespace CsvToDatabaseAbstraction.Models
{
    public class ColumnParameterMapping
    {
        public ColumnParameterMapping(string columnName = null, string columnKey = null, string parameterKey = null)
        {
            ColumnName = columnName;
            ColumnKey = columnKey;
            ParameterKey = parameterKey;
        }

        public string ColumnName { get; set; }
        public string ColumnKey { get; set; }
        public string ParameterKey { get; set; }
    }
}
