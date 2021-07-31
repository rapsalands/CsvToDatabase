using CsvToDatabaseAbstraction.Models;
using System.Collections.Generic;

namespace CsvToDatabaseAbstraction
{
    public abstract class DbQuery
    {
        public string DatabasePath { get; }

        public DbQuery(string databasePath)
        {
            DatabasePath = databasePath;
        }

        public abstract string Insert(TableOption tableOption, List<ColumnParameterMapping> columnParameterMappings);

        public abstract string CreateTable(TableOption tableOption);
    }
}
