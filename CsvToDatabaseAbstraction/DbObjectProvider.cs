using System;
using System.Data.Common;

namespace CsvToDatabaseAbstraction
{
    public abstract class DbObjectProvider
    {
        /// <summary>
        /// Database Path
        /// </summary>
        public string DatabasePath { get; }

        /// <summary>
        /// DB Command object.
        /// </summary>
        public DbCommand DbCommand { get; }

        /// <summary>
        /// DB Connection.
        /// </summary>
        public DbConnection DbConnection { get; }

        /// <summary>
        /// DbC Transaction.
        /// </summary>
        private DbTransaction dbTransaction { get; set; }

        /// <summary>
        /// Constructor. Modifies database name based on extension passed.
        /// </summary>
        /// <param name="databasePath"></param>
        public DbObjectProvider(string databasePath)
        {
            if (!databasePath.EndsWith(".sqlite"))
                databasePath = $"{databasePath}.sqlite";

            DatabasePath = databasePath;

            DbCommand = SetDbCommand();
            DbConnection = SetDbConnection();
        }

        /// <summary>
        /// DbConnection to be used for executing queries.
        /// </summary>
        /// <returns></returns>
        public abstract DbConnection SetDbConnection();
        /// <summary>
        /// DbCommand to be used for executing queries.
        /// </summary>
        /// <returns></returns>
        public abstract DbCommand SetDbCommand();

        /// <summary>
        /// DbParameter instance to avoid SQL injection.
        /// </summary>
        /// <returns></returns>
        public abstract DbParameter DbParameter();

        /// <summary>
        /// Returns command object after attaching SQL query and connection.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DbCommand GetDbCommand(string sql)
        {
            var command = DbCommand;
            command.CommandText = sql;
            command.Connection = DbConnection;
            return command;
        }

        /// <summary>
        /// Open connection and begin transaction.
        /// </summary>
        public void Begin()
        {
            DbConnection.Open();
            dbTransaction = DbConnection.BeginTransaction();
        }

        /// <summary>
        /// Commit transaction in try/catch. Rollback if failed.
        /// </summary>
        /// <returns></returns>
        public bool Commit()
        {
            try
            {
                dbTransaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                dbTransaction.Rollback();
                return false;
            }
        }
    }
}
