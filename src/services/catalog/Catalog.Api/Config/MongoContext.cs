using GreenShop.Catalog.Api.Config.Interfaces;
using GreenShop.Catalog.Api.Properties;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace GreenShop.Catalog.Api.Utils
{
    internal class MongoContext : IMongoContext
    {
        public IMongoClient Client { get; private set; }
        public IMongoDatabase Database { get; private set; }

        public MongoContext(IConfiguration configuration)
        {
            Client = new MongoClient(configuration.GetSection($"{Resources.Connection}:{Resources.MongoSection}:{Resources.MongoConnectionString}").Value);
            Database = Client.GetDatabase(Resources.MongoCatalog);
        }
    }
}
