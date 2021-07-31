using CsvToDatabaseAbstraction;
using System;
using System.Data.Common;
using System.IO;
using UnitTests.Helper;

namespace UnitTests.Common
{
    public abstract class BaseTest : IDisposable
    {
        private string databasePath;

        public SchemaTestHelper SchemaTestHelper { get; }
        public SchemaAssert SchemaAssert { get; }

        public BaseTest()
        {
            SchemaTestHelper = new SchemaTestHelper(ProviderInvariantName(), DbProviderFactory());
            SchemaAssert = new SchemaAssert(ColumnNameIndex());
        }

        public abstract string ToDbType(CsvToDatabase csvToDatabase);
        public abstract string ProviderInvariantName();
        public abstract DbProviderFactory DbProviderFactory();
        public virtual int ColumnNameIndex() => 3;

        public string DatabasePath()
        {
            databasePath = $"DumpData/{Guid.NewGuid()}.sqlite";
            return databasePath;
        }

        public void Dispose()
        {
            if(File.Exists(databasePath))
            {
                File.Delete(databasePath);
            }
        }
    }
}
