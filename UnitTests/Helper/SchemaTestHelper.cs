using System.Data;
using System.Linq;
using System.Data.Common;
using System.Collections.Generic;

namespace UnitTests.Helper
{
    public class SchemaTestHelper
    {
        private readonly string providerInvariantName;
        private readonly DbProviderFactory providerFactory;

        public SchemaTestHelper(string providerInvariantName, DbProviderFactory providerFactory)
        {
            this.providerInvariantName = providerInvariantName;
            this.providerFactory = providerFactory;
        }

        public DataTable GetTableSchema(string databasePath) => GetSchema(databasePath, new[] { SchemaTypeEnum.Tables }).First();
        public DataTable GetColumnsSchema(string databasePath) => GetSchema(databasePath, new[] { SchemaTypeEnum.Columns }).First();
        public List<DataTable> GetAllSchema(string databasePath) => GetSchema(databasePath, new[] { SchemaTypeEnum.Columns, SchemaTypeEnum.Tables });

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
