using CsvToDatabaseAbstraction.Models;
using System.Collections.Generic;

namespace CsvToDatabaseAbstraction
{
    /// <summary>
    /// Provides database query related values
    /// </summary>
    public abstract class DbQuery
    {
        /// <summary>
        /// Database path
        /// </summary>
        public string DatabasePath { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="databasePath">Database path</param>
        public DbQuery(string databasePath)
        {
            DatabasePath = databasePath;
        }

        /// <summary>
        /// Generate Insert query for the database using the passed in parameters.
        /// </summary>
        /// <returns>Insert query</returns>
        public abstract string Insert(TableOption tableOption, List<ColumnParameterMapping> columnParameterMappings);

        /// <summary>
        /// Implement functionality to generate database.
        /// </summary>
        /// <returns>Database path</returns>
        public abstract string CreateTable(TableOption tableOption);
    }
}
