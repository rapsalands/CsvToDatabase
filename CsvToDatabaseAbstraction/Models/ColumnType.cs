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
        /// <summary>
        /// Boolean
        /// </summary>
        Bit = 1,

        /// <summary>
        /// DateTime
        /// </summary>
        DateTime = 3,
        
        /// <summary>
        /// Interger/Number
        /// </summary>
        Int = 4,
        
        /// <summary>
        /// Text
        /// </summary>
        VarChar = 6,
    }
}
