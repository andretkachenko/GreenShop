using Dapper.Contrib.Extensions;
using GreenShop.Catalog.Models.Categories;
using GreenShop.Catalog.Models.Comments;
using GreenShop.Catalog.Models.Specifications;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace GreenShop.Catalog.Models.Products
{
    public class Product : IProduct
    {
        [BsonIgnore]
        public int Id { get; set; }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string MongoId { get; set; }
        [BsonIgnore]
        public string Name { get; set; }

        [BsonIgnore]
        public string Description { get; set; }

        [BsonIgnore]
        public decimal BasePrice { get; set; }
        [BsonIgnore]
        public float Rating { get; set; }

        [BsonIgnore]
        public int CategoryId { get; set; }

        [Write(false), BsonIgnore]
        public Category Category { get; set; }
        [Write(false), JsonIgnore, BsonIgnore]
        public IEnumerable<IComment> Comments { get; set; }
        [BsonElement("specifications"), Write(false)]
        public IEnumerable<Specification> Specifications { get; set; }
        
        /// <summary>
        /// Check if specified Product has any properties filled, that are stored in MongoDB
        /// </summary>
        public bool HasMongoProperties()
        {
            return Specifications != null && Specifications.Any();
        }

        /// <summary>
        /// Check if specified Product has any properties filled, that are stored in SQL DB
        /// </summary>
        public bool HasSqlProperties()
        {
            return !string.IsNullOrWhiteSpace(Name)
                    || CategoryId != 0
                    || !string.IsNullOrWhiteSpace(Description)
                    || BasePrice != 0
                    || Rating != 0;
        }
    }
}
