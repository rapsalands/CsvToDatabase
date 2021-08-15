using System;
using CsvToDatabaseAbstraction.Models;
using System.Collections.Generic;
using System.IO;
using CsvToDatabaseAbstraction.Helpers;

namespace CsvToDatabaseAbstraction
{
    /// <summary>
    /// Entry level class to invoke the funcionality.
    /// </summary>
    public class CsvToDatabase
    {
        private readonly Csv2DbOption csv2DbOption;
        private readonly Validate validate;
        private readonly FileSystem fileSystem;
        private readonly CsvUtility csvUtility;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="csv2DbOption"></param>
        public CsvToDatabase(Csv2DbOption csv2DbOption)
        {
            this.csv2DbOption = csv2DbOption;
            validate = new Validate();
            fileSystem = new FileSystem();
            csvUtility = new CsvUtility();
        }

        /// <summary>
        /// Usually this method is called by one of the related implementation nugets.
        /// Generates the database based on user options and returns database path.
        /// </summary>
        public string GenerateDb(Func<string, CsvToDbWorkflow> csvToDbWorkflowFunc)
        {
            if (csvToDbWorkflowFunc == null) throw new ArgumentNullException("Function cannot be null. Instance of CsvToDbWorkflow is needed to proceed.");

            validate.AssertDatabasePath(csv2DbOption.DatabasePath);

            var csvToDbWorkflow = csvToDbWorkflowFunc(csv2DbOption.DatabasePath);

            if (string.IsNullOrWhiteSpace(csv2DbOption.SourcePath))
                throw new ArgumentNullException($"Source path cannot be null or empty.");

            if (File.Exists(csv2DbOption.SourcePath))
                return FromFile(csv2DbOption, csvToDbWorkflow);

            return FromDirectory(csv2DbOption, csvToDbWorkflow);
        }

        /// <summary>
        /// Generate database and create table based on the file path passed.
        /// </summary>
        /// <param name="csv2DbOption"></param>
        /// <returns></returns>
        private string FromFile(Csv2DbOption csv2DbOption, CsvToDbWorkflow csvToDbWorkflow)
        {
            return Invoke(csvToDbWorkflow, csv2DbOption, new List<string> { csv2DbOption.SourcePath });
        }

        /// <summary>
        /// Generate database and create tables based on all the CSV files passed in folder/subfolder.
        /// </summary>
        /// <param name="csv2DbOption"></param>
        /// <returns></returns>
        private string FromDirectory(Csv2DbOption csv2DbOption, CsvToDbWorkflow csvToDbWorkflow)
        {
            var filePaths = fileSystem.GetCsvFiles(csv2DbOption.SourcePath);
            return Invoke(csvToDbWorkflow, csv2DbOption, filePaths);
        }

        /// <summary>
        /// Actual implementation of creating database and tables.
        /// </summary>
        /// <param name="csv2DbOption"></param>
        /// <param name="filePaths"></param>
        /// <returns></returns>
        private string Invoke(CsvToDbWorkflow csvToDbWorkflow, Csv2DbOption csv2DbOption, IEnumerable<string> filePaths)
        {
            if (csv2DbOption.SkipDatabaseIfExist && csvToDbWorkflow.DatabaseExists(csv2DbOption))
                return csv2DbOption.DatabasePath;

            var databasePath = csvToDbWorkflow.CreateDatabase(csv2DbOption);

            csvToDbWorkflow.Begin();

            var needCustomization = csv2DbOption.CustomizeTableData != null;

            List<TableOption> tableOptions = CreateTableOptionsAndActualTableIfNeeded(csvToDbWorkflow, csv2DbOption, filePaths, needCustomization);

            if (needCustomization)
            {
                tableOptions = csv2DbOption.CustomizeTableData(tableOptions);
                foreach (var tableOption in tableOptions) CreateTableAndPopulateData(csvToDbWorkflow, csv2DbOption, tableOption);
            }

            CommitAndDispose(csvToDbWorkflow, databasePath);

            return databasePath;
        }

        /// <summary>
        /// Builds table options.
        /// If they DOT NOT NEED customization then builds actual table and populate data if needed.
        /// If they NEED customization then returns tableOptions without creating tables.
        /// </summary>
        /// <param name="csv2DbOption"></param>
        /// <param name="filePaths"></param>
        /// <param name="needCustomization">Flag to denote if end user wants to customize table options data.</param>
        /// <returns></returns>
        private List<TableOption> CreateTableOptionsAndActualTableIfNeeded(CsvToDbWorkflow csvToDbWorkflow, Csv2DbOption csv2DbOption, IEnumerable<string> filePaths, bool needCustomization)
        {
            var tableOptions = new List<TableOption>();

            foreach (var filePath in filePaths)
            {
                var records = csvUtility.Read<object>(filePath);
                var tableOption = csvUtility.BuildTableOption(filePath, records, csv2DbOption);

                if (needCustomization) tableOptions.Add(tableOption);
                else CreateTableAndPopulateData(csvToDbWorkflow, csv2DbOption, tableOption);
            }

            return tableOptions;
        }

        /// <summary>
        /// Create table and populate data in it based on user flag "PopulateData";
        /// </summary>
        /// <param name="csv2DbOption"></param>
        /// <param name="tableOption"></param>
        private void CreateTableAndPopulateData(CsvToDbWorkflow csvToDbWorkflow, Csv2DbOption csv2DbOption, TableOption tableOption)
        {
            csvToDbWorkflow.CreateTable(tableOption);

            if (csv2DbOption.PopulateData)
            {
                var records = csvUtility.Read<object>(tableOption.FilePath);
                csvToDbWorkflow.PopulateDataInTable(tableOption, records);
            }
        }

        /// <summary>
        /// Commit transaction, if failed delete the database.
        /// Try closing and disposing Connection and Command.
        /// </summary>
        /// <param name="databasePath"></param>
        private void CommitAndDispose(CsvToDbWorkflow csvToDbWorkflow, string databasePath)
        {
            var commited = csvToDbWorkflow.Commit();
            if (!commited) File.Delete(databasePath);

            try
            {
                csvToDbWorkflow.DbConnection.Close();
                csvToDbWorkflow.DbConnection.Dispose();
                csvToDbWorkflow.DbCommand.Dispose();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
