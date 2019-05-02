using MongoDB.Bson;

namespace GreenShop.Catalog.Api.Helpers
{
    public static class MongoHelper
    {
        /// <summary>
        /// Create ObjectId to make Primary Key for Mongo BSON Document
        /// </summary>
        /// <returns>ObjectId, converted to string</returns>
        public static string GenerateMongoId()
        {
            return ObjectId.GenerateNewId().ToString();
        }
    }
}
