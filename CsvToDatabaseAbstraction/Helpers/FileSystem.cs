using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace CsvToDatabaseAbstraction.Helpers
{
    /// <summary>
    /// utility functions for FileSystem.
    /// </summary>
    public class FileSystem
    {
        private readonly Validate validate;

        /// <summary>
        /// Constructor
        /// </summary>
        public FileSystem()
        {
            validate = new Validate();
        }

        /// <summary>
        /// Get CSV files from folder and its subfolders.
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public List<string> GetCsvFiles(string directoryPath)
        {
            validate.AssertDirectoryPath(directoryPath);

            var files = Directory.EnumerateFiles(directoryPath, "*.*", SearchOption.AllDirectories)
            .Where(s => s.EndsWith(".csv"))
            .ToList();

            return files;
        }
    }
}
