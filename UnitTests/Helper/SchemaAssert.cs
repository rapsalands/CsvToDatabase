using System;
using System.Data;
using System.Linq;

namespace UnitTests.Helper
{
    /// <summary>
    /// Schema assertion.
    /// </summary>
    public class SchemaAssert
    {
        public int ColumnNameIndex { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="columnNameIndex"></param>
        public SchemaAssert(int columnNameIndex)
        {
            ColumnNameIndex = columnNameIndex;
        }

        /// <summary>
        /// Return column if found, else null.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="name"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private DataRow Found(DataTable dataTable, string name, int index)
        {
            var found = dataTable.AsEnumerable().FirstOrDefault(n => n.ItemArray[index].ToString() == name);
            return found;
        }

        /// <summary>
        /// Throws error if column found.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public DataRow ColumnNotFound(DataTable dataTable, string name)
        {
            var found = Found(dataTable, name, ColumnNameIndex);
            if (found != null) throw new MissingFieldException($"Column {name} is missing.");
            return found;
        }

        /// <summary>
        /// Throws error if column not found.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public DataRow ColumnFound(DataTable dataTable, string name)
        {
            var found = Found(dataTable, name, ColumnNameIndex);
            if (found == null) throw new MissingFieldException($"Column {name} is missing.");
            return found;
        }
    }
}
