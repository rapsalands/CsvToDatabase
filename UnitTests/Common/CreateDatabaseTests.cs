using Xunit;
using System.IO;
using CsvToDatabaseAbstraction;
using CsvToDatabaseAbstraction.Models;
using CsvToDatabaseAbstraction.Helpers;
using System;

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

            csvToDb = new CsvToDatabase(option, new Validate());
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
            csvToDb = new CsvToDatabase(option, new Validate());
            var databasePath = ToDbType(csvToDb);
            Assert.True(File.Exists(databasePath));
        }

        [Fact]
        public void FailedNameEmpty()
        {
            var option = new Csv2DbOption
            {
                DatabasePath = "",
                SourcePath = @"DumpData"
            };

            option.PopulateData = true;
            csvToDb = new CsvToDatabase(option, new Validate());
            Assert.Throws<ArgumentNullException>(() => ToDbType(csvToDb));
        }

        [Fact]
        public void FailedNameNull()
        {
            var option = new Csv2DbOption
            {
                DatabasePath = null,
                SourcePath = @"DumpData"
            };

            option.PopulateData = true;
            csvToDb = new CsvToDatabase(option, new Validate());
            Assert.Throws<ArgumentNullException>(() => ToDbType(csvToDb));
        }

        [Fact]
        public void CreateWithNoExtension()
        {
            var option = new Csv2DbOption
            {
                DatabasePath = "DumpData/Sample",
                SourcePath = @"DumpData"
            };

            option.PopulateData = true;
            csvToDb = new CsvToDatabase(option, new Validate());
            var databasePath = ToDbType(csvToDb);
            Assert.True(File.Exists(databasePath));
        }

        [Fact]
        public void CreateAndSkip()
        {
            var option = new Csv2DbOption
            {
                DatabasePath = "DumpData/Sample",
                SourcePath = @"DumpData",
            };

            csvToDb = new CsvToDatabase(option, new Validate());
            var databasePath = ToDbType(csvToDb);
            Assert.True(File.Exists(databasePath));

            option.DatabasePath = databasePath;
            option.SkipDatabaseIfExist = true;
            csvToDb = new CsvToDatabase(option, new Validate());
            databasePath = ToDbType(csvToDb);
            Assert.True(File.Exists(databasePath));
        }
    }
}
