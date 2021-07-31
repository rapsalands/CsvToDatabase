using System;
using CsvToDatabaseAbstraction.Models;
using System.Collections.Generic;
using System.IO;
using CsvToDatabaseAbstraction.Helpers;

namespace CsvToDatabaseAbstraction
{
    public class CsvToDatabase
    {
        private readonly Csv2DbOption csv2DbOption;
        private readonly FileSystem fileSystem;
        private readonly CsvUtility csvUtility;
        private CsvToDbWorkflow csvToDbWorkflow;

        public CsvToDatabase(Csv2DbOption csv2DbOption)
        {
            this.csv2DbOption = csv2DbOption;

            fileSystem = new FileSystem();
            csvUtility = new CsvUtility();
        }

        /// <summary>
        /// Usually this method is called by one of the related implementation.
        /// </summary>
        public string GenerateDb(Func<string, CsvToDbWorkflow> csvToDbWorkflowFunc)
        {
            if (csvToDbWorkflowFunc == null)
                throw new ArgumentNullException("Function cannot be null. Instance of CsvToDbWorkflow is needed to proceed.");

            csvToDbWorkflow = csvToDbWorkflowFunc(csv2DbOption.DatabasePath);

            if (string.IsNullOrWhiteSpace(csv2DbOption.SourcePath))
                throw new ArgumentNullException($"Source path cannot be null or empty.");

            if (File.Exists(csv2DbOption.SourcePath))
                return FromFile(csv2DbOption);

            return FromDirectory(csv2DbOption);
        }

        private string FromFile(Csv2DbOption csv2DbOption)
        {
            return Invoke(csv2DbOption, new List<string> { csv2DbOption.SourcePath });
        }

        private string FromDirectory(Csv2DbOption csv2DbOption)
        {
            var filePaths = fileSystem.GetCsvFiles(csv2DbOption.SourcePath);
            return Invoke(csv2DbOption, filePaths);
        }

        private string Invoke(Csv2DbOption csv2DbOption, IEnumerable<string> filePaths)
        {
            var databasePath = csvToDbWorkflow.CreateDatabase();

            csvToDbWorkflow.Begin();

            foreach (var filePath in filePaths)
            {
                var records = csvUtility.Read<object>(filePath);

                var tableOption = csvUtility.BuildTableOption(filePath, records, csv2DbOption);
                csvToDbWorkflow.CreateTable(tableOption);

                if (csv2DbOption.LoadData)
                {
                    csvToDbWorkflow.PopulateDataInTable(tableOption, records);
                }
            }

            var commited = csvToDbWorkflow.Commit();
            if (!commited) File.Delete(databasePath);

            return databasePath;
        }
    }
}
