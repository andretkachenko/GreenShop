using GreenShop.Catalog.Config.Interfaces;
using GreenShop.Catalog.Infrastructure.Products.Interfaces;
using GreenShop.Catalog.Domain.Products;
using GreenShop.Catalog.Properties;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Infrastructure.Products
{
    public class MongoProductRepository : IMongoProductRepository
    {
        private readonly IMongoContext _mongoContext;

        /// <summary>
        /// Get Mongo Collection for Products
        /// </summary>
        private IMongoCollection<Product> MongoCollection => _mongoContext.Database.GetCollection<Product>(Resources.Products);

        public MongoProductRepository(IMongoContext mongoContext)
        {
            _mongoContext = mongoContext;
        }

        /// <summary>
        /// Asynchronously get all Products
        /// </summary>
        /// <returns>Task with list of all Products</returns>
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            List<Product> products = await MongoCollection.Find(_ => true).ToListAsync();

            return products;
        }

        /// <summary>
        /// Asynchronously get Product with the specific id
        /// </summary>
        /// <param name="id">Id of the Product to get</param>
        /// <returns>Task with specified Product</returns>
        public async Task<Product> GetAsync(string id)
        {
            Product product = await MongoCollection.Find(x => x.MongoId == id).FirstOrDefaultAsync();

            return product;
        }

        /// <summary>
        /// Asynchronously adds Product
        /// </summary>
        /// <param name="product">Product to add</param>
        /// <returns>Task with specified Category</returns>
        public async Task<bool> CreateAsync(Product product)
        {
            await MongoCollection.InsertOneAsync(product);
            return true;
        }

        /// <summary>
        /// Asynchronously remove Product with specified id
        /// </summary>
        /// <param name="id">Id of the Product to delete</param>
        /// <returns>Number of rows affected</returns>
        public async Task<bool> DeleteAsync(string id)
        {
            await MongoCollection.FindOneAndDeleteAsync(x => x.MongoId == id);
            return true;
        }

        /// <summary>
        /// Asynchronously update specified Product
        /// </summary>
        /// <param name="product">Product, that contains id of entity that should be changed, and all changed values</param>
        /// <returns>Number of rows affected</returns>
        public async Task<bool> UpdateAsync(Product product)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(x => x.MongoId, product.MongoId);
            await MongoCollection.FindOneAndReplaceAsync(filter, product);
            return true;
        }

        public IDbTransaction Transaction => throw new System.NotImplementedException();
        public void SetSqlTransaction(IDbTransaction transaction) => throw new System.NotImplementedException();
    }
}
