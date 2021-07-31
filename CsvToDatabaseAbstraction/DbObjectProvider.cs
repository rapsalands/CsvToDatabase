using System;
using System.Data.Common;

namespace CsvToDatabaseAbstraction
{
    public abstract class DbObjectProvider
    {
        public string DatabasePath { get; }
        public DbCommand DbCommand { get; }
        public DbConnection DbConnection { get; }
        private DbTransaction dbTransaction { get; set; }

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

        public DbCommand GetDbCommand(string sql)
        {
            var command = DbCommand;
            command.CommandText = sql;
            command.Connection = DbConnection;
            return command;
        }

        public void Begin()
        {
            DbConnection.Open();
            dbTransaction = DbConnection.BeginTransaction();
        }

        public bool Commit()
        {
            try
            {
                dbTransaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                dbTransaction.Rollback();
                return false;
            }
        }
    }
}
