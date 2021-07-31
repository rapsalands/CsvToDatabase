using CsvToDatabaseAbstraction;

namespace CsvToSqlite
{
    public static class CsvToDbExtension
    {
        /// <summary>
        /// Converts CSV to Sqlite Database.
        /// </summary>
        /// <param name="csvToDb"></param>
        public static string ToSqlite(this CsvToDatabase csvToDb)
        {
            return csvToDb.GenerateDb((databasePath) => new ToSqliteWorkflow(databasePath));
        }
    }
}
