using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace CsvToDatabaseAbstraction.Helpers
{
    public class FileSystem
    {
        private readonly Validate validate;

        public FileSystem()
        {
            validate = new Validate();
        }

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
