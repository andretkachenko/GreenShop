using Common.Models.Specifications;
using Common.Models.Categories.Interfaces;
using Common.Models.Comments.Interfaces;
using Common.Models.Products.Interfaces;
using Common.Validatiors;
using Dapper.Contrib.Extensions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

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
        
        [Write(false), JsonIgnore, BsonIgnore]
        public ICategory Category { get; set; }
        [Write(false), JsonIgnore, BsonIgnore]
        public IEnumerable<IComment> Comments { get; set; }
        [BsonElement("specifications"), Write(false)]
        public IEnumerable<Specification> Specifications { get; set; }

        public override bool Equals(object obj)
        {
            // If parameter is null or cannot be cast to Category return false.
            if (obj == null || !(obj is Product that))
            {
                return false;
            }

            // Return true if the fields match:
            return EqualityValidator.ReflectiveEquals(this, that);
        }

        public bool Equals(Product obj)
        {
            // If parameter is null return false:
            if (obj == null)
            {
                return false;
            }

            // Return true if the fields match:
            return EqualityValidator.ReflectiveEquals(this, obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, CategoryId);
        }
    }
}
