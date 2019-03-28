using GreenShop.Catalog.Config.Interfaces;
using GreenShop.Catalog.DataAccessors.Interfaces;
using GreenShop.Catalog.Models.Products;
using GreenShop.Catalog.Properties;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenShop.Catalog.DataAccessors
{
    public class MongoProducts : IMongoDataAccessor<Product>
    {
        private readonly IMongoContext _mongoContext;

        /// <summary>
        /// Get Mongo Collection for Products
        /// </summary>
        private IMongoCollection<Product> MongoCollection => _mongoContext.Database.GetCollection<Product>(Resources.Products);

        public MongoProducts(IMongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }

        /// <summary>
        /// Asynchronously gets all Products
        /// </summary>
        /// <returns>Task with list of all Products</returns>
        public async Task<IEnumerable<Product>> GetAll()
        {
            List<Product> products = await MongoCollection.Find(_ => true).ToListAsync();

            return products;
        }

        /// <summary>
        /// Asynchronously gets Product with the specific id
        /// </summary>
        /// <param name="id">Id of the Product to get</param>
        /// <returns>Task with specified Product</returns>
        public async Task<Product> Get(string id)
        {
            Product product = await MongoCollection.Find(x => x.MongoId == id).FirstOrDefaultAsync();

            return product;
        }

        /// <summary>
        /// Asynchronously adds Product
        /// </summary>
        /// <param name="product">Product to add</param>
        /// <returns>Task with specified Category</returns>
        public async Task Add(Product product)
        {
            await MongoCollection.InsertOneAsync(product);
        }

        /// <summary>
        /// Asynchronously removed Product with specified id
        /// </summary>
        /// <param name="id">Id of the Product to delete</param>
        /// <returns>Number of rows affected</returns>
        public async Task Delete(string id)
        {
            await MongoCollection.FindOneAndDeleteAsync(x => x.MongoId == id);
        }

        /// <summary>
        /// Asynchronously edits specified Product
        /// </summary>
        /// <param name="product">Product, that contains id of entity that should be changed, and all changed values</param>
        /// <returns>Number of rows affected</returns>
        public async Task Edit(Product product)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(x => x.MongoId, product.MongoId);
            await MongoCollection.FindOneAndReplaceAsync(filter, product);
        }
    }
}
