using Xunit;
using CsvToDatabaseAbstraction.Models;
using CsvToDatabaseAbstraction;
using CsvToSqlite;
using UnitTests.Helper;
using CsvToDatabaseAbstraction.Helpers;

namespace UnitTests.Common
{
    /// <summary>
    /// Tests related to primary column in table.
    /// </summary>
    public abstract partial class CommonTests : BaseTest
    {
        [Fact]
        public void IdAutomaticTest()
        {
            var csvOptions = new Csv2DbOption
            {
                DatabasePath = DatabasePath(),
                SourcePath = @"DumpData"
            };

            var csv = new CsvToDatabase(csvOptions, new Validate());
            string databasePath = ToDbType(csv);

            var schema = SchemaTestHelper.GetColumnsSchema(databasePath);
            SchemaAssert.ColumnFound(schema, "Id");
        }

        [Fact]
        public void NullIdTest()
        {
            var csvOptions = new Csv2DbOption
            {
                DatabasePath = DatabasePath(),
                SourcePath = @"DumpData",
                AddPrimaryColumnAs = null
            };

            var csv = new CsvToDatabase(csvOptions, new Validate());
            string databasePath = ToDbType(csv);

            var schema = SchemaTestHelper.GetColumnsSchema(databasePath);
            SchemaAssert.ColumnNotFound(schema, "Id");
        }

        [Fact]
        public void EmptyIdTest()
        {
            var csvOptions = new Csv2DbOption
            {
                DatabasePath = DatabasePath(),
                SourcePath = @"DumpData",
                AddPrimaryColumnAs = string.Empty
            };

            var csv = new CsvToDatabase(csvOptions, new Validate());
            string databasePath = ToDbType(csv);

            var schema = SchemaTestHelper.GetColumnsSchema(databasePath);
            SchemaAssert.ColumnNotFound(schema, "Id");
        }

        [Fact]
        public void ExplicitIdTest()
        {
            var csvOptions = new Csv2DbOption
            {
                DatabasePath = DatabasePath(),
                SourcePath = @"DumpData",
                AddPrimaryColumnAs = "TableId"
            };

            var csv = new CsvToDatabase(csvOptions, new Validate());
            string databasePath = ToDbType(csv);

            var schema = SchemaTestHelper.GetColumnsSchema(databasePath);
            SchemaAssert.ColumnFound(schema, "TableId");
            SchemaAssert.ColumnNotFound(schema, "Id");
        }
    }
}
