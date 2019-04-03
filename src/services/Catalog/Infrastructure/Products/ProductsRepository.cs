using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using GreenShop.Catalog.Config.Interfaces;
using GreenShop.Catalog.Models.Products;
using GreenShop.Catalog.Properties;
using MongoDB.Driver;

namespace GreenShop.Catalog.Infrastructure.Products
{
    public class ProductsRepository : IRepository<Product>
    {
        private ISqlContext SqlContext;
        private readonly IMongoContext MongoContext;
        private IMongoCollection<Product> MongoCollection => MongoContext.Database.GetCollection<Product>(Resources.Products);
        public IDbTransaction Transaction { get; private set; }

        public ProductsRepository(ISqlContext sqlContext, IMongoContext mongoContext)
        {
            SqlContext = sqlContext;
            MongoContext = mongoContext;
        }

        public void SetTransaction(IDbTransaction transaction)
        {
            Transaction = transaction;
        }
        
        /// <summary>
        /// Asynchronously get all Products
        /// </summary>
        /// <returns>Task with list of all Products</returns>
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            Task<IEnumerable<Product>> sqlGetAllTask = SqlProducts.GetAll();
            Task<IEnumerable<Product>> mongoGetAllTask = MongoProducts.GetAll();
            List<Task> taskList = new List<Task>
            {
                sqlGetAllTask,
                mongoGetAllTask
            };
            await Task.WhenAll(taskList);

            IEnumerable<Product> products = MergeProducts(sqlGetAllTask.Result, mongoGetAllTask.Result);
            return products;

            using (SqlConnection context = SqlContext.Connection)
            {
                IEnumerable<Product> products = await context.GetAllAsync<Product>();

                return products;
            }
        }
        
        /// <summary>
        /// Asynchronously get Product with the specific id
        /// </summary>
        /// <param name="id">Id of the Product to get</param>
        /// <returns>Task with specified Product</returns>
        public async Task<Product> GetAsync(string id)
        {
            using (SqlConnection context = SqlContext.Connection)
            {
                Product product = await context.GetAsync<Product>(id);

                return product;
            }
        }
        
        /// <summary>
        /// Asynchronously create Product
        /// </summary>
        /// <param name="product">Product to add</param>
        /// <returns>Task with specified Product</returns>
        public async Task<int> CreateAsync(Product product)
        {
            using (SqlConnection context = SqlContext.Connection)
            {
                int id = await context.InsertAsync(product, transaction: Transaction);

                return id;
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            using (SqlConnection context = SqlContext.Connection)
            {
                int affectedRows = await context.ExecuteAsync(@"
                    DELETE
                    FROM [Products]
                    WHERE [Id] = @id
                ", new
                {
                    id
                }, transaction: Transaction);

                return affectedRows == 1;
            }
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            using (SqlConnection context = SqlContext.Connection)
            {
                string query = @"
                    UPDATE [Products]
                    SET
                    ";

                if (!string.IsNullOrWhiteSpace(product.Name))
                {
                    query += " [Name] = @name";
                }
                if (product.CategoryId != null)
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

                int affectedRows = await context.ExecuteAsync(query, new
                {
                    id = product.Id,
                    name = product.Name,
                    parentId = product.CategoryId,
                    description = product.Description,
                    basePrice = product.BasePrice,
                    rating = product.Rating
                });

                return affectedRows == 1;
            }
        }        
        
        /// <summary>
        /// Get MongoId field for a Product with the specified Id
        /// </summary>
        /// <param name="id">Id of a Product</param>
        /// <returns>MongoId</returns>
        private string GetMongoId(string id)
        {
            using (SqlConnection context = SqlContext.Connection)
            {
                string mongoId = context.Query<string>(@"
                    SELECT [MongoId]
                    FROM [Products]
                    WHERE [Id] = @id
                ", new
                {
                    id
                }).FirstOrDefault();

                return mongoId;
            }
        }

        /// <summary>
        /// Merge Lists of Products from SQL and MongoDB
        /// </summary>
        /// <param name="sqlProducts">List of Products from SQL DB</param>
        /// <param name="mongoProducts">List of Products from MongoDB</param>
        /// <returns>List of merged Products</returns>
        private IEnumerable<Product> MergeProducts(IEnumerable<Product> sqlProducts, IEnumerable<Product> mongoProducts)
        {
            List<Product> products = new List<Product>();

            foreach (Product sqlProduct in sqlProducts)
            {
                products.Add(MergeProduct(sqlProduct, mongoProducts.FirstOrDefault(x => x.MongoId == sqlProduct.MongoId)));
            }

            return products;
        }

        /// <summary>
        /// Merge Products from SQL and MongoDB
        /// </summary>
        /// <param name="sqlProduct">Product from SQL DB</param>
        /// <param name="mongoProduct">Product from MongoDB</param>
        /// <returns>Merged Product</returns>
        private Product MergeProduct(Product sqlProduct, Product mongoProduct)
        {
            if (sqlProduct == null)
            {
                throw new ArgumentNullException(Resources.NullSqlProductException);
            }
            if (mongoProduct == null)
            {
                return sqlProduct;
            }
            if (sqlProduct.MongoId != mongoProduct.MongoId)
            {
                throw new ArgumentException(Resources.ProductsMismatchException);
            }

            sqlProduct.Specifications = mongoProduct.Specifications;

            return sqlProduct;
        }
    }
}
