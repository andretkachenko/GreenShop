using Catalog.Properties;
using Common.Configuration.MongoDB;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;

namespace Catalog.Utils
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
