using Xunit;
using System.IO;
using CsvToDatabaseAbstraction;
using CsvToDatabaseAbstraction.Models;

namespace UnitTests.Common
{
    /// <summary>
    /// Tests related to creation of database.
    /// </summary>
    public abstract partial class CommonTests : BaseTest
    {
        private Csv2DbOption option;
        private CsvToDatabase csvToDb;

        public CommonTests()
        {
        }

        [Fact]
        public void CreateDbOnly()
        {
            var option = new Csv2DbOption
            {
                DatabasePath = DatabasePath(),
                SourcePath = @"DumpData"
            };

            csvToDb = new CsvToDatabase(option);
            var databasePath = ToDbType(csvToDb);
            Assert.True(File.Exists(databasePath));
        }

        [Fact]
        public void LoadData()
        {
            var option = new Csv2DbOption
            {
                DatabasePath = DatabasePath(),
                SourcePath = @"DumpData"
            };

            option.PopulateData = true;
            csvToDb = new CsvToDatabase(option);
            var databasePath = ToDbType(csvToDb);
            Assert.True(File.Exists(databasePath));
        }
    }
}
