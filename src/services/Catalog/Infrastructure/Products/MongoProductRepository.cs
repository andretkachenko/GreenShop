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

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            List<Product> products = await MongoCollection.Find(_ => true).ToListAsync();

            return products;
        }
        
        public async Task<Product> GetAsync(string id)
        {
            Product product = await MongoCollection.Find(x => x.MongoId == id).FirstOrDefaultAsync();

            return product;
        }
        
        public async Task<bool> CreateAsync(Product product)
        {
            await MongoCollection.InsertOneAsync(product);
            return true;
        }
        
        public async Task<bool> DeleteAsync(string id)
        {
            await MongoCollection.FindOneAndDeleteAsync(x => x.MongoId == id);
            return true;
        }
        
        public async Task<bool> UpdateAsync(Product product)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(x => x.MongoId, product.MongoId);
            await MongoCollection.FindOneAndReplaceAsync(filter, product);
            return true;
        }
    }
}
