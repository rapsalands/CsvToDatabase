using Xunit;
using System.IO;
using CsvToDatabaseAbstraction;
using CsvToDatabaseAbstraction.Models;

namespace UnitTests.Common
{
    public abstract class CreateDatabaseTests : BaseTest
    {
        private Csv2DbOption option;
        private CsvToDatabase csvToDb;

        public CreateDatabaseTests()
        {
            option = new Csv2DbOption
            {
                DatabasePath = DatabasePath(),
                SourcePath = @"DumpData"
            };
        }

        [Fact]
        public void CreateDbOnly()
        {
            csvToDb = new CsvToDatabase(option);
            var databasePath = ToDbType(csvToDb);
            Assert.True(File.Exists(databasePath));
        }

        [Fact]
        public void LoadData()
        {
            option.LoadData = true;
            csvToDb = new CsvToDatabase(option);
            var databasePath = ToDbType(csvToDb);
            Assert.True(File.Exists(databasePath));
        }
    }
}
