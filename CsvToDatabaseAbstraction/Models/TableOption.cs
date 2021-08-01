using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CsvToDatabaseAbstraction.Models
{
    /// <summary>
    /// Table Options to create database table.
    /// </summary>
    public class TableOption
    {
        private string name;

        /// <summary>
        /// Constructor
        /// </summary>
        public TableOption()
        {
            name = Path.GetFileNameWithoutExtension(FilePath);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="columnDefinitions"></param>
        public TableOption(string filePath, List<ColumnDefinition> columnDefinitions = null)
        {
            FilePath = filePath;
            ColumnDefinitions = columnDefinitions;
        }

        /// <summary>
        /// CSV File path to create database table.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// CSV file name with extension.
        /// </summary>
        public string FileName => Path.GetFileName(FilePath);

        /// <summary>
        /// Table name (CSV file name without extension).
        /// </summary>
        public string Name { get => name; set => name = value; }

        /// <summary>
        /// Column definitions for this table.
        /// </summary>
        public List<ColumnDefinition> ColumnDefinitions { get; set; }

        /// <summary>
        /// Returns Primary column is exists else null.
        /// </summary>
        public ColumnDefinition Primary
        {
            get
            {
                if (ColumnDefinitions == null) return null;
                return ColumnDefinitions.SingleOrDefault(n => n.IsPrimary);
            }
        }

        /// <summary>
        /// Returns if table has primary column.
        /// </summary>
        public bool HasPrimary => Primary != null;
    }
}
