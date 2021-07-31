using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CsvToDatabaseAbstraction.Models
{
    public class TableOption
    {
        public TableOption()
        {

        }

        public TableOption(string filePath, List<ColumnDefinition> columnDefinitions = null)
        {
            FilePath = filePath;
            ColumnDefinitions = columnDefinitions;
        }

        public string FilePath { get; set; }
        public string FileName => Path.GetFileName(FilePath);
        public string FileNameNoExtension => Path.GetFileNameWithoutExtension(FilePath);

        public string Identity { get; set; }

        public List<ColumnDefinition> ColumnDefinitions { get; set; }

        public ColumnDefinition Primary
        {
            get
            {
                if (ColumnDefinitions == null) return null;
                return ColumnDefinitions.SingleOrDefault(n => n.IsPrimary);
            }
        }

        public bool HasPrimary => Primary != null;
    }
}
