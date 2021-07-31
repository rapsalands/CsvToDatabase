using CsvToDatabaseAbstraction.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace CsvToDatabaseAbstraction.Helpers
{
    public class CsvUtility
    {
        private readonly Validate validate;
        private readonly ColumnUtility columnUtility;

        public CsvUtility()
        {
            validate = new Validate();
            columnUtility = new ColumnUtility();
        }

        public List<T> Read<T>(string filePath)
        {
            validate.AssertFilePath(filePath);

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                BadDataFound = (args) => BadDataFoundHandler(filePath, args),
            };

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, config);
            var records = csv.GetRecords<T>();
            return records.ToList();
        }

        private void BadDataFoundHandler(string filePath, BadDataFoundArgs args)
        {

        }

        public List<string> GetColumnNames(string filePath)
        {
            validate.AssertFilePath(filePath);

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.Read();
            csv.ReadHeader();

            string[] headerRow = csv.HeaderRecord;
            return headerRow.ToList();
        }

        public TableOption BuildTableOption(string filePath, List<object> records, Csv2DbOption csv2DbOption)
        {
            validate.AssertFilePath(filePath);

            var columns = GetColumnNames(filePath);
            var definitions = columnUtility.CreateDefinitions(columns);

            columnUtility.ConfigureDefinitions(records, definitions);

            AddPrimaryKeyColumn(csv2DbOption, definitions);

            var tableOption = new TableOption(filePath, definitions);

            return tableOption;
        }

        private void AddPrimaryKeyColumn(Csv2DbOption csv2DbOption, List<ColumnDefinition> definitions)
        {
            if (string.IsNullOrWhiteSpace(csv2DbOption.AddPrimaryColumnAs)) return;
            if (columnUtility.HasPrimary(definitions, csv2DbOption.AddPrimaryColumnAs)) return;

            definitions.Insert(0, columnUtility.DefaultPrimaryColumn(csv2DbOption.AddPrimaryColumnAs));
        }
    }
}
