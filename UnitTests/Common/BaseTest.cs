using System;
using System.IO;
using UnitTests.Helper;
using System.Data.Common;
using CsvToDatabaseAbstraction;

namespace UnitTests.Common
{
    /// <summary>
    /// Base Test class.
    /// </summary>
    public abstract class BaseTest : IDisposable
    {
        private string databasePath;

        /// <summary>
        /// Schema Test Helper.
        /// </summary>
        public SchemaTestHelper SchemaTestHelper { get; }

        /// <summary>
        /// Schema assertion class.
        /// </summary>
        public SchemaAssert SchemaAssert { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public BaseTest()
        {
            SchemaTestHelper = new SchemaTestHelper(ProviderInvariantName(), DbProviderFactory());
            SchemaAssert = new SchemaAssert(ColumnNameIndex());
        }

        /// <summary>
        /// Returns database path. Usually provides a method to invoke functionality.
        /// As an example, provide method as ToSqlite().
        /// </summary>
        /// <param name="csvToDatabase"></param>
        /// <returns></returns>
        public abstract string ToDbType(CsvToDatabase csvToDatabase);

        /// <summary>
        /// Provider Invariant Name. Usually needed for fetching schema of database via code.
        /// </summary>
        /// <returns></returns>
        public abstract string ProviderInvariantName();

        /// <summary>
        /// DBProviderFactory instance. Usually needed for fetching schema of database via code.
        /// </summary>
        /// <returns></returns>
        public abstract DbProviderFactory DbProviderFactory();

        /// <summary>
        /// Index of Item array where column name exists.
        /// </summary>
        /// <returns></returns>
        public virtual int ColumnNameIndex() => 3;

        /// <summary>
        /// database Path.
        /// </summary>
        /// <returns></returns>
        public string DatabasePath()
        {
            databasePath = $"DumpData/{Guid.NewGuid()}.sqlite";
            return databasePath;
        }

        /// <summary>
        /// Delete database at end of each unit test.
        /// </summary>
        public void Dispose()
        {
            if(File.Exists(databasePath))
            {
                File.Delete(databasePath);
            }
        }
    }
}
