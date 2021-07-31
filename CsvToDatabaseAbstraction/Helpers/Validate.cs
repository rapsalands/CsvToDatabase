using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CsvToDatabaseAbstraction.Helpers
{
    public class Validate
    {
        public void AssertFilePath(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File {filePath} not found.");
        }

        public void AssertDirectoryPath(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Directory {directoryPath} not found.");
        }
    }
}
