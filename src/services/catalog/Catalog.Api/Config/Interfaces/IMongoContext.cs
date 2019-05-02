using MongoDB.Driver;

namespace GreenShop.Catalog.Api.Config.Interfaces
{
    public interface IMongoContext
    {
        IMongoClient Client { get; }
        IMongoDatabase Database { get; }
    }
}
