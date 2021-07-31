using CsvToDatabaseAbstraction;
using CsvToSqlite;
using System.Data.Common;
using System.Data.SQLite;
using UnitTests.Common;

namespace UnitTests.SQLite
{
    public class SqliteCreateDatabaseTests : CommonTests
    {
        public override DbProviderFactory DbProviderFactory() => SQLiteFactory.Instance;
        public override string ProviderInvariantName() => "System.Data.SQLite";
        public override string ToDbType(CsvToDatabase csvToDatabase) => csvToDatabase.ToSqlite();
    }
}
