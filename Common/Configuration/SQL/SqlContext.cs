using System.Data.SqlClient;

namespace Common.Configuration.SQL
{
    /// <summary>
    /// Abstract class, describing basic logic for SqlContext.
    /// Getter for Context should be overriden in order to use establish proper connection
    /// </summary>
    public abstract class SqlContext
    {
        /// <summary>
        /// Reads configuration file and returns connection string assembled from it
        /// </summary>
        protected static SqlConnection Context { get; }

        /// <summary>
        /// Assembles connection string from configuration file
        /// </summary>
        /// <param name="dataSource">Url to the SQL server</param>
        /// <param name="initCatalog">Used database</param>
        /// <returns>Connection string</returns>
        /// <remarks>Should be changed later on to work with different types of authentication</remarks>
        protected static string AssembleConnectionString(string dataSource, string initCatalog) 
            => $"Data Source={dataSource};Initial Catalog={initCatalog};Integrated Security=True";
    }
}
