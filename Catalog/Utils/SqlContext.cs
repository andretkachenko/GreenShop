using Catalog.Properties;
using Common.Configuration.SQL;
using System;
using System.Data.SqlClient;

namespace Catalog.Utils
{
    public sealed class SqlContext : BaseSqlContext
    {
        /// <summary>
        /// Reads environment variables that store database source and name
        /// </summary>
        public new static SqlConnection Context
        {
            get
            {
                var dataSource = Environment.GetEnvironmentVariable(Resources.DataSource);
                var initialCatalog = Environment.GetEnvironmentVariable(Resources.InitialCatalog);
                var connectionString = AssembleConnectionString(dataSource, initialCatalog);
                return new SqlConnection(connectionString);
            }
        }
    }
}
