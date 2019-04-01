using GreenShop.Catalog.Config;
using GreenShop.Catalog.Config.Interfaces;
using GreenShop.Catalog.Properties;
using Microsoft.Extensions.Configuration;
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
                IConfigurationSection section = _configuration.GetSection($"{Resources.Connection}:{Resources.SqlSection}");
                string dataSource = section.GetSection($"{Resources.DataSource}").Value;
                string initialCatalog = section.GetSection($"{Resources.InitialCatalog}").Value;
                string connectionString = AssembleConnectionString(dataSource, initialCatalog);
                return new SqlConnection(connectionString);
            }
        }
    }
}
