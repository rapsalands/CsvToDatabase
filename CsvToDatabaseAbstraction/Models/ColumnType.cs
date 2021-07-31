using System;
using System.Collections.Generic;
using System.Text;

namespace CsvToDatabaseAbstraction.Models
{
    /// <summary>
    /// Column Types. All databases may not support all types.
    /// Columns will evaluated in increasing order of enum values.
    /// </summary>
    public enum ColumnType
    {
        Bit = 1,
        DateTime = 3,
        Int = 4,
        VarChar = 6,
    }
}
