using CsvToDatabaseAbstraction.Models;
using System.Collections.Generic;
using System.Data.Common;
using System.Dynamic;

namespace CsvToDatabaseAbstraction
{
    /// <summary>
    /// Workflow for creating and loading database.
    /// </summary>
    public abstract class CsvToDbWorkflow : DbObjectProvider
    {
        public CsvToDbWorkflow(string databasePath) : base(databasePath)
        {
        }

        /// <summary>
        /// DbQuery provides required database connection objects and SQL commands.
        /// </summary>
        public abstract DbQuery DbQuery { get; }

        /// <summary>
        /// Creates empty database.
        /// </summary>
        /// <returns></returns>
        public abstract string CreateDatabase();

        /// <summary>
        /// Create table based on options passed.
        /// </summary>
        /// <param name="tableOption"></param>
        public void CreateTable(TableOption tableOption)
        {
            string sql = DbQuery.CreateTable(tableOption);

            var command = GetDbCommand(sql);

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Load data in table of database.
        /// </summary>
        /// <param name="tableOption"></param>
        /// <param name="records"></param>
        public void PopulateDataInTable(TableOption tableOption, List<object> records)
        {
            var colParamMapping = ColumnParamMapping(tableOption);

            string sql = DbQuery.Insert(tableOption, colParamMapping);

            var command = GetDbCommand(sql);

            var parameters = new Dictionary<string, DbParameter>();

            foreach (var mapping in colParamMapping)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = mapping.ParameterKey;
                command.Parameters.Add(parameter);
                parameters.Add(mapping.ColumnName, parameter);
            }

            foreach (ExpandoObject recordObject in records)
            {
                var record = new Dictionary<string, object>(recordObject);

                foreach (var mapping in colParamMapping)
                {
                    parameters[mapping.ColumnName].Value = record[mapping.ColumnKey];
                }

                command.ExecuteNonQuery();
            }
        }

        private List<ColumnParameterMapping> ColumnParamMapping(TableOption tableOption)
        {
            var result = new List<ColumnParameterMapping>();

            foreach (var cd in tableOption.ColumnDefinitions)
            {
                if (cd.IsPrimary && cd.Identity) continue;
                result.Add(new ColumnParameterMapping(cd.Name, cd.Key, $"${cd.Name}"));
            }

            return result;
        }
    }
}
