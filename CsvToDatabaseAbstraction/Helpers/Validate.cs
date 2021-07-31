using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CsvToDatabaseAbstraction.Helpers
{
    /// <summary>
    /// Validation Service
    /// </summary>
    public class Validate
    {
        /// <summary>
        /// Asserts if file exists.
        /// </summary>
        /// <param name="filePath"></param>
        public void AssertFilePath(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File {filePath} not found.");
        }

        /// <summary>
        /// Asserts if directory exists.
        /// </summary>
        /// <param name="directoryPath"></param>
        public void AssertDirectoryPath(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Directory {directoryPath} not found.");
        }
    }
}
