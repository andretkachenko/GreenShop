using Dapper.Contrib.Extensions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace GreenShop.Catalog.Domain.Products
{
    public class Product : IAggregate
    {
        #region Constructors
        /// <summary>
        /// Controller used by the Dapper in order to map obtain from DB
        /// values into thr Enitty model.
        /// Apart from this use-case, it should never be called.
        /// </summary>
        private Product() { }

        public Product(string name, int categoryId) : this(name, categoryId, null) { }
        public Product(string name, int categoryId, string description)
        {
            Name = name;
            CategoryId = categoryId;
            Description = description;
        }
        #endregion

        #region Properties
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

        [Write(false), JsonIgnore, BsonIgnore]
        public IEnumerable<Comment> Comments { get; set; }
        [BsonElement("specifications"), Write(false)]
        public IEnumerable<Specification> Specifications { get; set; }
        #endregion

        #region Setters
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
        /// Add or update Specifications for the Product
        /// </summary>
        /// <param name="specifications">Updated list of Specifcations</param>
        public void UpdateSpecifications(IEnumerable<Specification> specifications)
        {
            Specifications = specifications;
        }

        /// <summary>
        /// Add Comment to the Product
        /// </summary>
        /// <param name="comment">New Comment to add to the Product's Comment list.</param>
        public void AddComment(Comment comment)
        {
            if (Comments == null) Comments = new List<Comment>();
            Comments.ToList().Add(comment);
        }

        /// <summary>
        /// Set id that was assigned to the product in the MongoDB
        /// </summary>
        /// <param name="id">Bson ObjectID that identifies product</param>
        public void SetMongoId(string id)
        {
            MongoId = id;
        }
        #endregion

        #region State Checkers
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
                    || CategoryId != default(int)
                    || !string.IsNullOrWhiteSpace(Description)
                    || BasePrice != 0
                    || Rating != 0;
        }
        #endregion
    }
}
