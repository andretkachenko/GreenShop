using MongoDB.Driver;

namespace GreenShop.Catalog.Config.Interfaces
{
    public interface IMongoContext
    {
        IMongoDatabase Database { get; }
    }
}
