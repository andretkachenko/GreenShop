using GreenShop.Catalog.Api.Config.Interfaces;
using GreenShop.Catalog.Api.Properties;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace GreenShop.Catalog.Api.Utils
{
    internal sealed class SqlContext : ISqlContext
    {
        private readonly IConfiguration _configuration;
        private SqlConnection _connection;

        public SqlContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Reads environment variables that store database source and name
        /// </summary>
        public SqlConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    IConfigurationSection section = _configuration.GetSection($"{Resources.Connection}:{Resources.SqlSection}");
                    string dataSource = section.GetSection($"{Resources.DataSource}").Value;
                    string initialCatalog = section.GetSection($"{Resources.InitialCatalog}").Value;
                    string connectionString = AssembleConnectionString(dataSource, initialCatalog);
                    _connection = new SqlConnection(connectionString);
                }
                return _connection;
            }
        }

        /// <summary>
        /// Assembles connection string from configuration file
        /// </summary>
        /// <param name="dataSource">Url to the SQL server</param>
        /// <param name="initCatalog">Used database</param>
        /// <returns>Connection string</returns>
        /// <remarks>Should be changed later on to work with different types of authentication</remarks>
        private string AssembleConnectionString(string dataSource, string initCatalog)
            => $"Data Source={dataSource};Initial Catalog={initCatalog};Integrated Security=True";

        private bool disposedValue = false;

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_connection != null)
                    {
                        _connection.Dispose();
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
