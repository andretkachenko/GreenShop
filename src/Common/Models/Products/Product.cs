using Common.Models.Specifications;
using Common.Models.Categories.Interfaces;
using Common.Models.Comments.Interfaces;
using Common.Models.Products.Interfaces;
using Dapper.Contrib.Extensions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;
using Common.Models.Categories;

namespace Common.Models.Products
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
    }
}
