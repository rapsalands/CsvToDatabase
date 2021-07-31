# CsvToSqlite

[![NuGet version (CsvToSqlite)](https://img.shields.io/nuget/v/CsvToSqlite.svg?style=flat-square)](https://www.nuget.org/packages/CsvToSqlite/)

#### Usage
```
    var csvOptions = new Csv2DbOption
    {
        DatabasePath = "DumpData/Sample", // database path (with or without extension, prefered without extension)
        SourcePath = @"DumpData" // Folder (and subfolders) path with all CSV files or single CSV file path
    };
    var csvToDb = new CsvToDatabase(csvOptions);
    csvToDb.ToSqlite();
```

---

# CsvToDatabase

[![NuGet version (CsvToDatabase)](https://img.shields.io/nuget/v/CsvToDatabase.svg?style=flat-square)](https://www.nuget.org/packages/CsvToDatabase/)

This library does provides the base functionality for converting CSV files to database. This library need not be used when using related packages like `CsvToSqlite`.

However if we do not have any related package for the database (currently for example SQL Server or Oracle), then only this library is recommended to be used.

#### Usage
```
var csvToDatabase = new CsvToDatabase();
csvToDatabase.GenerateDb(() => new NewCsvToDbWorkflow()); // NewCsvToDbWorkflow is implementation of CsvToDbWorkflow (see below).
```

#### CsvToDbWorkflow
```
public abstract class CsvToDbWorkflow
{
    /// <summary>
    /// DbQuery provides required database connection objects and SQL commands.
    /// </summary>
    public abstract DbQuery DbQuery { get; }

    /// <summary>
    /// Creates empty database.
    /// </summary>
    /// <returns>Database path</returns>
    public abstract string CreateDatabase();
}

public abstract class DbQuery
{
    public DbQuery(string databasePath)
    {
    }

    /// <summary>
    /// Generate Insert query for the database using the passed in parameters.
    /// </summary>
    /// <returns>Insert query</returns>
    public abstract string Insert(TableOption tableOption, List<ColumnParameterMapping> columnParameterMappings);
    
    /// <summary>
    /// Implement functionality to generate database.
    /// </summary>
    /// <returns>Database path</returns>
    public abstract string CreateTable(TableOption tableOption);
}
```
