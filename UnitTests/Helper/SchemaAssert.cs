using System;
using System.Data;
using System.Linq;

namespace UnitTests.Helper
{
    public class SchemaAssert
    {
        public int ColumnNameIndex { get; }

        public SchemaAssert(int columnNameIndex)
        {
            ColumnNameIndex = columnNameIndex;
        }

        private DataRow Found(DataTable dataTable, string name, int index)
        {
            var found = dataTable.AsEnumerable().FirstOrDefault(n => n.ItemArray[index].ToString() == name);
            return found;
        }

        public DataRow ColumnNotFound(DataTable dataTable, string name)
        {
            var found = Found(dataTable, name, ColumnNameIndex);
            if (found != null) throw new MissingFieldException($"Column {name} is missing.");
            return found;
        }

        public DataRow ColumnFound(DataTable dataTable, string name)
        {
            var found = Found(dataTable, name, ColumnNameIndex);
            if (found == null) throw new MissingFieldException($"Column {name} is missing.");
            return found;
        }
    }
}
