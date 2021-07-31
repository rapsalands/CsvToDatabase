using System;
using System.Collections.Generic;
using System.Text;

namespace CsvToDatabaseAbstraction.Models
{
    public class Csv2DbOption
    {
        public Csv2DbOption()
        {

        }

        public Csv2DbOption(string sourcePath, string databasePath)
        {
            SourcePath = sourcePath;
            DatabasePath = databasePath;
        }

        /// <summary>
        /// Database path. Excluding database extension.
        /// </summary>
        public string DatabasePath { get; set; }

        /// <summary>
        /// Can be file path or folder path.
        /// </summary>
        public string SourcePath { get; set; }

        /// <summary>
        /// Load data after creating database is true.
        /// </summary>
        public bool LoadData { get; set; }
    }
}
