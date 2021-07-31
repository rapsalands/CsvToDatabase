using Xunit;
using CsvToSqlite;
using CsvToDatabaseAbstraction.Models;
using CsvToDatabaseAbstraction;

namespace UnitTests
{
    public class CsvToSqliteTests
    {
        private Csv2DbOption option;
        private CsvToDatabase csvToDb;

        public CsvToSqliteTests()
        {
            option = new Csv2DbOption
            {
                DatabasePath = "DumpData/Sample",
                SourcePath = @"DumpData"
            };
        }

        [Fact]
        public void CreateDbOnly()
        {
            csvToDb = new CsvToDatabase(option);
            csvToDb.ToSqlite();
        }

        [Fact]
        public void LoadData()
        {
            option.LoadData = true;
            csvToDb = new CsvToDatabase(option);
            csvToDb.ToSqlite();
        }
    }
}
