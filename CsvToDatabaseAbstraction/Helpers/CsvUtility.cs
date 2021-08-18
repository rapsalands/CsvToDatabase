using CsvToDatabaseAbstraction.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace CsvToDatabaseAbstraction.Helpers
{
    /// <summary>
    /// Utility functions for CSV.
    /// </summary>
    public class CsvUtility
    {
        private readonly Validate validate;
        private readonly ColumnUtility columnUtility;

        /// <summary>
        /// Constructor
        /// </summary>
        public CsvUtility()
        {
            validate = new Validate();
            columnUtility = new ColumnUtility();
        }

        /// <summary>
        /// Read data from a CSV file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Handler for bad data.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="args"></param>
        private void BadDataFoundHandler(string filePath, BadDataFoundArgs args)
        {

        }

        /// <summary>
        /// Get Column header names from file passed.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Build table Option based on the paramerters.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="records"></param>
        /// <param name="csv2DbOption"></param>
        /// <returns></returns>
        public TableOption BuildTableOption(string filePath, List<object> records, Csv2DbOption csv2DbOption)
        {
            validate.AssertFilePath(filePath);

            var columns = GetColumnNames(filePath);
            columns = columns.Where(n => !string.IsNullOrWhiteSpace(n)).ToList();
            var definitions = columnUtility.CreateDefinitions(columns);

            columnUtility.ConfigureDefinitions(records, definitions);

            AddPrimaryKeyColumn(csv2DbOption, definitions);

            var tableOption = new TableOption(filePath, definitions);

            return tableOption;
        }

        /// <summary>
        /// Add primary column if needed.
        /// </summary>
        /// <param name="csv2DbOption"></param>
        /// <param name="definitions"></param>
        private void AddPrimaryKeyColumn(Csv2DbOption csv2DbOption, List<ColumnDefinition> definitions)
        {
            if (string.IsNullOrWhiteSpace(csv2DbOption.AddPrimaryColumnAs)) return;
            if (columnUtility.HasPrimary(definitions, csv2DbOption.AddPrimaryColumnAs)) return;

            definitions.Insert(0, columnUtility.DefaultPrimaryColumn(csv2DbOption.AddPrimaryColumnAs));
        }
    }
}
