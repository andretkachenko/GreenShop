using MongoDB.Driver;

namespace Common.Configuration.MongoDB
{
    public interface IMongoContext
    {
        IMongoDatabase Database { get; }
    }
}
