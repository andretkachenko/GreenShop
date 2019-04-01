using Dapper.Contrib.Extensions;
using GreenShop.Catalog.Models.Categories;
using GreenShop.Catalog.Models.Comments;
using GreenShop.Catalog.Models.Specifications;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GreenShop.Catalog.Models.Products
{
    public class Product : IAggregate
    {
        public Product(string name, Guid categoryId) : this(name, categoryId, null) { }
        public Product(string name, Guid categoryId, string description)
        {
            Name = name;
            CategoryId = categoryId;
            Description = description;
        }

        [BsonIgnore]
        public Guid Id { get; protected set; }
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string MongoId { get; protected set; }
        [BsonIgnore]
        public string Name { get; protected set; }

        [BsonIgnore]
        public string Description { get; protected set; }

        [BsonIgnore]
        public decimal BasePrice { get; protected set; }
        [BsonIgnore]
        public float Rating { get; protected set; }

        [BsonIgnore]
        public Guid CategoryId { get; protected set; }

        [Write(false), BsonIgnore]
        public Category Category { get; set; }
        [Write(false), JsonIgnore, BsonIgnore]
        public IEnumerable<Comment> Comments { get; set; }
        [BsonElement("specifications"), Write(false)]
        public IEnumerable<Specification> Specifications { get; set; }

        /// <summary>
        /// Change Rating of the Product to the specified value
        /// </summary>
        /// <param name="newRating">New rating value</param>
        public void UpdateRating(float newRating)
        {
            Rating = newRating;
        }

        /// <summary>
        /// Change Description of the Product
        /// </summary>
        /// <param name="newDescription">New Description</param>
        public void UpdateDescription(string newDescription)
        {
             Description = newDescription;
        }

        /// <summary>
        /// Update Base Price of the Product
        /// </summary>
        /// <param name="newBasePrice">New price value</param>
        public void UpdateBasePrice(decimal newBasePrice)
        {
            BasePrice = newBasePrice;
        }

        /// <summary>
        /// Move Product to the different Category
        /// </summary>
        /// <param name="newCategoryId">ID of the new Category</param>
        public void ChangeCategory(Guid newCategoryId)
        {
            CategoryId = newCategoryId;
        }
        
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
                    || CategoryId != null
                    || !string.IsNullOrWhiteSpace(Description)
                    || BasePrice != 0
                    || Rating != 0;
        }
    }
}
