using System.Data.Common;

namespace CsvToDatabaseAbstraction.Models
{
    public class ColumnParameterMapping
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="columnName">Formatted column name</param>
        /// <param name="columnKey">CSV file column name</param>
        /// <param name="parameterKey">Query insert parameter</param>
        public ColumnParameterMapping(string columnName = null, string columnKey = null, string parameterKey = null)
        {
            ColumnName = columnName;
            ColumnKey = columnKey;
            ParameterKey = parameterKey;
        }

        /// <summary>
        /// Formatted Column Name to be used to generate column in database table.
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Actual column name mentioned in CSV. Used to retrieve data when needed.
        /// </summary>
        public string ColumnKey { get; set; }

        /// <summary>
        /// Parameter name used in Insert query to avoid SQL injection.
        /// </summary>
        public string ParameterKey { get; set; }
    }
}
