using MongoDB.Driver;

namespace GreenShop.Catalog.Config.Interfaces
{
    public interface IMongoContext
    {
        IMongoClient Client { get; }
        IMongoDatabase Database { get; }
    }
}
