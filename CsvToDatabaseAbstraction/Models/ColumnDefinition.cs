using System;

namespace CsvToDatabaseAbstraction.Models
{
    /// <summary>
    /// Column Definitions used to build table columns.
    /// </summary>
    public class ColumnDefinition
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ColumnDefinition()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Column name</param>
        public ColumnDefinition(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Column name</param>
        /// <param name="columnType">Column type</param>
        public ColumnDefinition(string name, ColumnType columnType)
        {
            Name = name;
            ColumnType = columnType;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Column name</param>
        /// <param name="isPrimary">Is Primary key Column</param>
        /// <param name="identity">If identity column. Considered only for primary key column.</param>
        public ColumnDefinition(string name, bool isPrimary, bool identity = true)
        {
            Name = name;
            IsPrimary = isPrimary;
            Identity = identity;
            ColumnType = ColumnType.Int;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Column nam</param>
        /// <param name="columnType">Column type</param>
        /// <param name="maxLength">Max length of column. Considered only for text based columns.</param>
        /// <param name="isPrimary">Is column primary key</param>
        public ColumnDefinition(string name, ColumnType columnType, int maxLength, bool isPrimary)
        {
            Name = name;
            ColumnType = columnType;
            MaxLength = maxLength;
            IsPrimary = isPrimary;
        }

        /// <summary>
        /// Original unformatted column header.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Column Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Is Primary Key.
        /// </summary>
        public bool IsPrimary { get; set; }

        /// <summary>
        /// True by default is IsPrimary is true.
        /// </summary>
        public bool Identity { get; set; }

        /// <summary>
        /// Do column supports null.
        /// </summary>
        public bool IsNull { get; set; }

        /// <summary>
        /// Has Unique Constraint.
        /// </summary>
        public bool IsUnique { get; set; }

        /// <summary>
        /// Need index.
        /// </summary>
        public bool Indexed { get; set; }

        /// <summary>
        /// Column Type.
        /// </summary>
        public ColumnType ColumnType { get; set; }

        /// <summary>
        /// Max Length of Column
        /// </summary>
        public int MaxLength { get; set; } = 1000;

        /// <summary>
        /// Set column type and retains max value till now.
        /// </summary>
        /// <param name="columnType"></param>
        public void SetMaxColumnType(ColumnType columnType)
        {
            ColumnType = (ColumnType)Math.Max((int)ColumnType, (int)columnType);
        }

        /// <summary>
        /// Set Is null.
        /// </summary>
        /// <param name="value"></param>
        public void SetIsNull(string value)
        {
            if (IsNull) return;

            var isNull = string.IsNullOrWhiteSpace(value);
            IsNull = isNull;
        }
    }
}
