using CsvToDatabaseAbstraction;

namespace CsvToSqlite
{
    public static class CsvToDbExtension
    {
        /// <summary>
        /// Extension method on CsvToDatabase to convert CSV to Sqlite Database.
        /// </summary>
        /// <param name="csvToDb"></param>
        public static string ToSqlite(this CsvToDatabase csvToDb)
        {
            return csvToDb.GenerateDb((databasePath) => new ToSqliteWorkflow(databasePath));
        }
    }
}
