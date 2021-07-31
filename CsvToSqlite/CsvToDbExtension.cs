using CsvToDatabaseAbstraction;

namespace CsvToSqlite
{
    public static class CsvToDbExtension
    {
        /// <summary>
        /// Converts CSV to Sqlite Database.
        /// </summary>
        /// <param name="csvToDb"></param>
        public static void ToSqlite(this CsvToDatabase csvToDb)
        {
            csvToDb.GenerateDb((databasePath) => new ToSqliteWorkflow(databasePath));
        }
    }
}
