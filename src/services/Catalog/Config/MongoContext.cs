using GreenShop.Catalog.Config.Interfaces;
using GreenShop.Catalog.Properties;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace GreenShop.Catalog.Utils
{
    internal class MongoContext : IMongoContext
    {
        public IMongoDatabase Database { get; private set; }

        public MongoContext(IConfiguration configuration)
        {
            Database = new MongoClient(configuration.GetSection($"{Resources.Connection}:{Resources.MongoSection}:{Resources.MongoConnectionString}").Value)
                .GetDatabase(Resources.MongoCatalog);
        }
    }
}
