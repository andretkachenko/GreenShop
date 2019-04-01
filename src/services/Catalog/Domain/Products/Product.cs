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
        protected Product()
        {

        }

        [BsonIgnore]
        public int Id { get; protected set; }
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
        public int CategoryId { get; protected set; }

        [Write(false), BsonIgnore]
        public Category Category { get; set; }
        [Write(false), JsonIgnore, BsonIgnore]
        public IEnumerable<IComment> Comments { get; set; }
        [BsonElement("specifications"), Write(false)]
        public IEnumerable<Specification> Specifications { get; set; }
        
        /// <summary>
        /// Create Product with the specified Name and CategoryID
        /// </summary>
        /// <param name="name">Name used for the Product</param>
        /// <param name="categoryId">Category, which Product represents</param>
        /// <returns>Created Product</returns>
        public static Product Create(string name, int categoryId)
        {
            return Create(name, categoryId, null);
        }
        
        /// <summary>
        /// Create Product with the specified Name, CategoryID and Description
        /// </summary>
        /// <param name="name">Name used for the Product</param>
        /// <param name="categoryId">Category, which Product represents</param>
        /// <param name="description">Description used for the Product</param>
        /// <returns>Created Product</returns>
        public static Product Create(string name, int categoryId, string description)
        {
            return Create(name, categoryId, description, 0, 0);
        }

        /// <summary>
        /// Create Product with the following values
        /// </summary>
        /// <param name="name">Name used for the Product</param>
        /// <param name="categoryId">Category, which Product represents</param>
        /// <param name="description">Description used for the Product</param>
        /// <param name="basePrice">Base Price for the Product (not including taxes, discount, shipping prices, etc)</param>
        /// <param name="rating">Rating of the Product</param>
        /// <returns>Created Product</returns>
        public static Product Create(string name, int categoryId, string description, decimal basePrice, float rating)
        {
            Product product = new Product
            {
                Name = name,
                CategoryId = categoryId,
                Description = description,
                BasePrice = basePrice,
                Rating = rating
            };

            return product;
        }

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
        public void ChangeCategory(int newCategoryId)
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
                    || CategoryId != 0
                    || !string.IsNullOrWhiteSpace(Description)
                    || BasePrice != 0
                    || Rating != 0;
        }
    }
}
