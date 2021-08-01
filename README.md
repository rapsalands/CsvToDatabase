# CsvToSqlite

[![NuGet version (CsvToSqlite)](https://img.shields.io/nuget/v/CsvToSqlite.svg?style=flat-square)](https://www.nuget.org/packages/CsvToSqlite/)

## Usage
```
    var csvOptions = new Csv2DbOption
    {
        DatabasePath = "DumpData/Sample", // database path (with or without extension, prefered without extension)
        SourcePath = @"DumpData" // Folder (and subfolders) path with all CSV files or single CSV file path
    };
    var csvToDb = new CsvToDatabase(csvOptions);
    csvToDb.ToSqlite();
```

## Customizing Database Creation WorkFlow (Csv2DbOption)

#### DatabasePath (string)
    - This attribute is to set database path where it needs to be created.
    - Value can be with or without database extension.

#### SourcePath (string)
    - This can be CSV file path. In this case a database will be created with single table. File name will be used as table name by default.
    - Or this can be direction path with single/multiple CSV files. In this case a database will be created with tables equals to the CSV files in provided directory/sub-directories.
    - Each table will have columns as per the header values in each CSV file.
    - Table and column configuration can be customized using `CustomizeTableData` function.

#### PopulateData (bool)
    - True: When set to true, populate data from CSV after creating database.
    - False: When set to false, empty data will be created.

#### AddPrimaryColumnAs (string)
    - Defaults to "Id". Means a primary key column with name "Id" will be added to every table provided no Id column is present in CSV file of that table.
    - Primary Key column are marked Identity and will hae seed of 1.
    - If this attribute is set to `null` or empty string then no primary key column will be added automatically.

#### List<TableOption> CustomizeTableData(List<TableOption>)
    - This method takes colleciton of tables option (and each tableOption corresponds to each CSV file).
    - Input arguments are untouched table option.
    - Chnage values as needed and return the changed table options.

---

# CsvToDatabase

[![NuGet version (CsvToDatabaseAbstraction)](https://img.shields.io/nuget/v/CsvToDatabaseAbstraction.svg?style=flat-square)](https://www.nuget.org/packages/CsvToDatabaseAbstraction/)

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
