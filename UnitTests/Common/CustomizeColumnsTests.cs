using Xunit;
using CsvToDatabaseAbstraction;
using CsvToDatabaseAbstraction.Models;
using System.Collections.Generic;
using System.Linq;
using CsvToDatabaseAbstraction.Helpers;

namespace UnitTests.Common
{
    public abstract partial class CommonTests : BaseTest
    {
        [Fact]
        public void NoCustomizationTest()
        {
            var csvOptions = new Csv2DbOption
            {
                DatabasePath = DatabasePath(),
                SourcePath = @"DumpData"
            };

            var csv = new CsvToDatabase(csvOptions);
            string databasePath = ToDbType(csv);

            var schema = SchemaTestHelper.GetColumnsSchema(databasePath);
            SchemaAssert.ColumnFound(schema, "City");
        }

        [Fact]
        public void CityColumnTest()
        {
            List<TableOption> CustomizeTableData(List<TableOption> tableOptions)
            {
                var cd = SchemaTestHelper.FindColumnDefinition(tableOptions.First(), "City");
                cd.Name = "CityName";
                return tableOptions;
            }

            var csvOptions = new Csv2DbOption
            {
                DatabasePath = DatabasePath(),
                SourcePath = @"DumpData",
                CustomizeTableData = CustomizeTableData
            };

            var csv = new CsvToDatabase(csvOptions);
            string databasePath = ToDbType(csv);

            var schema = SchemaTestHelper.GetColumnsSchema(databasePath);
            SchemaAssert.ColumnFound(schema, "CityName");
            SchemaAssert.ColumnNotFound(schema, "City");
        }
    }
}
