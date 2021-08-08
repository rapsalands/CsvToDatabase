using CsvToDatabaseAbstraction;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;

namespace CsvToSqlite
{
    /// <summary>
    /// Sqlite database workflow.
    /// </summary>
    public class ToSqliteWorkflow : CsvToDbWorkflow
    {
        /// <summary>
        /// Sqlite database workflow.
        /// </summary>
        /// <param name="databasePath">Database path. Path should exclude database extension.</param>
        public ToSqliteWorkflow(string databasePath) : base(databasePath)
        {
        }

        /// <summary>
        /// DBCommand to used to execute queries.
        /// </summary>
        /// <returns></returns>
        public override DbCommand SetDbCommand() => new SQLiteCommand();

        /// <summary>
        /// DBConnection to be used for executing queries.
        /// Calling this function explicitly is not recommended as this will create multiple instances of DbConnection.
        /// </summary>
        /// <returns></returns>
        public override DbConnection SetDbConnection()
        {
            return new SQLiteConnection($"Data Source={DatabasePath};Version=3;");
        }

        /// <summary>
        /// SQL Parameter.
        /// </summary>
        /// <returns></returns>
        public override DbParameter DbParameter() => new SQLiteParameter();

        /// <summary>
        /// Instance provides query related information.
        /// </summary>
        public override DbQuery DbQuery => new SqliteQuery(DatabasePath);

        /// <summary>
        /// Database extension.
        /// </summary>
        public override string DbExtension => "sqlite";

        /// <summary>
        /// Methos creates database.
        /// </summary>
        /// <returns>Returns database path.</returns>
        public override string CreateDatabase()
        {
            if (File.Exists(DatabasePath))
                File.Delete(DatabasePath);

            SQLiteConnection.CreateFile(DatabasePath);
            return DatabasePath;
        }
    }
}
