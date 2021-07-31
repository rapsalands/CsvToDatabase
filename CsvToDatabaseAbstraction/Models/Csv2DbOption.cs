namespace CsvToDatabaseAbstraction.Models
{
    /// <summary>
    /// Options object for converting CSV file/s into Database.
    /// </summary>
    public class Csv2DbOption
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Csv2DbOption()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sourcePath">Source path of files/folder containing CSV files</param>
        /// <param name="databasePath">Database path (with or without extension. Preferred without extension).</param>
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

        /// <summary>
        /// Defaults to "Id". Means a primary with identity with name "Id" will be added to every table.
        /// However new column will be added only if no column with provided name exists in table.
        /// If set as null or empty string, no column will be added.
        /// </summary>
        public string AddPrimaryColumnAs { get; set; } = "Id";
    }
}
