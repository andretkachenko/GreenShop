using Dapper;
using Dapper.Contrib.Extensions;
using GreenShop.Catalog.Config.Interfaces;
using GreenShop.Catalog.Domain.Products;
using GreenShop.Catalog.Infrastructure.Products.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Infrastructure.Products
{
    public class SqlProductRepository : ISqlProductRepository
    {
        private readonly ISqlContext _sql;

        public IDbTransaction Transaction { get; private set; }

        public SqlProductRepository(ISqlContext sqlContext)
        {
            _sql = sqlContext;
        }

        public void SetSqlTransaction(IDbTransaction transaction)
        {
            Transaction = transaction;
        }

        /// <summary>
        /// Asynchronously get all Products
        /// </summary>
        /// <returns>Task with list of all Products</returns>
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
                IEnumerable<Product> products = await _sql.Connection.GetAllAsync<Product>();

                return products;
        }

        /// <summary>
        /// Asynchronously get Product with the specific id
        /// </summary>
        /// <param name="id">Id of the Product to get</param>
        /// <returns>Task with specified Product</returns>
        public async Task<Product> GetAsync(int id)
        {
                Product product = await _sql.Connection.GetAsync<Product>(id);

                return product;
        }

        /// <summary>
        /// Get MongoId field for a Product with the specified Id
        /// </summary>
        /// <param name="id">Id of a Product</param>
        /// <returns>MongoId</returns>
        public async Task<string> GetMongoIdAsync(int id)
        {
                IEnumerable<string> result = await _sql.Connection.QueryAsync<string>(@"
                    SELECT [MongoId]
                    FROM [Products]
                    WHERE [Id] = @id
                ", new
                {
                    id
                });

                string mongoId = result.FirstOrDefault();

                return mongoId;
        }

        /// <summary>
        /// Asynchronously create Product
        /// </summary>
        /// <param name="product">Product to add</param>
        /// <returns>Result flag</returns>
        public async Task<int> CreateAsync(Product product)
        {
                int id = await _sql.Connection.InsertAsync(product, transaction: Transaction);

                return id;
        }

        /// <summary>
        /// Asynchronously remove Product with specified id
        /// </summary>
        /// <param name="id">Id of the Product to delete</param>
        /// <returns>Result flag</returns>
        public async Task<bool> DeleteAsync(int id)
        {
                await _sql.Connection.ExecuteAsync(@"
                    DELETE
                    FROM [Products]
                    WHERE [Id] = @id
                ", new
                {
                    id
                },
                transaction: Transaction);

                return true;
        }

        /// <summary>
        /// Asynchronously edit specified Product
        /// </summary>
        /// <param name="product">Product, that contains id of entity that should be changed, and all changed values</param>
        /// <returns>Result flag</returns>
        public async Task<bool> UpdateAsync(Product product)
        {
                string query = @"
                    UPDATE [Products]
                    SET
                    ";

                if (!string.IsNullOrWhiteSpace(product.Name))
                {
                    query += " [Name] = @name";
                }
                if (product.CategoryId != default(int))
                {
                    query += " [CategoryId] = @categoryId";
                }
                if (!string.IsNullOrWhiteSpace(product.Description))
                {
                    query += " [Description] = @description";
                }
                if (product.BasePrice != 0)
                {
                    query += " [BasePrice] = @basePrice";
                }
                if (product.Rating != 0)
                {
                    query += " [Rating] = @rating";
                }

                query += " WHERE [Id] = @id";

                int affectedRows = await _sql.Connection.ExecuteAsync(query, new
                {
                    id = product.Id,
                    name = product.Name,
                    parentId = product.CategoryId,
                    description = product.Description,
                    basePrice = product.BasePrice,
                    rating = product.Rating
                },
                transaction: Transaction);

                return true;
        }
    }
}