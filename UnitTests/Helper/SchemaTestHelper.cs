﻿using System.Data;
using System.Linq;
using System.Data.Common;
using System.Collections.Generic;

namespace UnitTests.Helper
{
    /// <summary>
    /// Schema Test Helper.
    /// </summary>
    public class SchemaTestHelper
    {
        private readonly string providerInvariantName;
        private readonly DbProviderFactory providerFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="providerInvariantName"></param>
        /// <param name="providerFactory"></param>
        public SchemaTestHelper(string providerInvariantName, DbProviderFactory providerFactory)
        {
            this.providerInvariantName = providerInvariantName;
            this.providerFactory = providerFactory;
        }

        /// <summary>
        /// Fetch table schema
        /// </summary>
        /// <param name="databasePath"></param>
        /// <returns></returns>
        public DataTable GetTableSchema(string databasePath) => GetSchema(databasePath, new[] { SchemaTypeEnum.Tables }).First();

        /// <summary>
        /// Fetch column related schema
        /// </summary>
        /// <param name="databasePath"></param>
        /// <returns></returns>
        public DataTable GetColumnsSchema(string databasePath) => GetSchema(databasePath, new[] { SchemaTypeEnum.Columns }).First();

        /// <summary>
        /// Fetch Table/Columns schema
        /// </summary>
        /// <param name="databasePath"></param>
        /// <returns></returns>
        public List<DataTable> GetAllSchema(string databasePath) => GetSchema(databasePath, new[] { SchemaTypeEnum.Columns, SchemaTypeEnum.Tables });

        /// <summary>
        /// Fetch schema based on arguements passed.
        /// </summary>
        /// <param name="databasePath"></param>
        /// <param name="schemaTypeEnums"></param>
        /// <returns></returns>
        public List<DataTable> GetSchema(string databasePath, SchemaTypeEnum[] schemaTypeEnums = null)
        {
            DbProviderFactories.RegisterFactory(providerInvariantName, providerFactory);

            DbProviderFactory factory = DbProviderFactories.GetFactory(providerInvariantName);
            using DbConnection connection = factory.CreateConnection();
            connection.ConnectionString = @$"Data Source={databasePath}";
            connection.Open();

            var result = new List<DataTable>();

            if (schemaTypeEnums.Any(n => n == SchemaTypeEnum.Tables)) result.Add(connection.GetSchema("Tables"));
            if (schemaTypeEnums.Any(n => n == SchemaTypeEnum.Columns)) result.Add(connection.GetSchema("Columns"));

            connection.Close();
            connection.Dispose();

            return result;
        }
    }
}