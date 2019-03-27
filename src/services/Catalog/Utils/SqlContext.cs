using GreenShop.Catalog.Properties;
using Common.Configuration.SQL;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;

namespace GreenShop.Catalog.Utils
{
    internal sealed class SqlContext : BaseSqlContext, ISqlContext
    {
        private readonly IConfiguration _configuration;

        public SqlContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Reads environment variables that store database source and name
        /// </summary>
        public new SqlConnection Context
        {
            get
            {
                var section = _configuration.GetSection($"{Resources.Connection}:{Resources.SqlSection}");
                var dataSource = section.GetSection($"{Resources.DataSource}").Value;
                var initialCatalog = section.GetSection($"{Resources.InitialCatalog}").Value;
                var connectionString = AssembleConnectionString(dataSource, initialCatalog);
                return new SqlConnection(connectionString);
            }
        }
    }
}
