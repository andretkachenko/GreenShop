using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using FluentValidation;
using GreenShop.Catalog.Config.Interfaces;
using GreenShop.Catalog.Helpers;
using GreenShop.Catalog.Infrastructure.Products.Interfaces;
using GreenShop.Catalog.Models.Products;
using GreenShop.Catalog.Properties;
using GreenShop.Catalog.Validators;
using MongoDB.Driver;

namespace GreenShop.Catalog.Infrastructure.Products
{
    public class ProductRepository : IRepository<Product>
    {
        private ISqlProducts SqlProducts;
        private IMongoProducts MongoProducts;
        private ISqlContext SqlContext;
        private readonly IMongoContext MongoContext;
        private IMongoCollection<Product> MongoCollection => MongoContext.Database.GetCollection<Product>(Resources.Products);
        public IDbTransaction Transaction { get; private set; }

        public ProductRepository(ISqlContext sqlContext, 
            IMongoContext mongoContext,
            ISqlProducts sqlProducts,
            IMongoProducts mongoProducts)
        {
            SqlContext = sqlContext;
            MongoContext = mongoContext;
            SqlProducts = sqlProducts;
            MongoProducts = mongoProducts;
        }

        public void SetSqlTransaction(IDbTransaction transaction)
        {
            Transaction = transaction;
        }
        
        /// <summary>
        /// Asynchronously gets all Products
        /// </summary>
        /// <returns>Task with list of all Products</returns>
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            Task<IEnumerable<Product>> sqlGetAllTask = SqlProducts.GetAllAsync();
            Task<IEnumerable<Product>> mongoGetAllTask = MongoProducts.GetAllAsync();
            List<Task> taskList = new List<Task>
            {
                sqlGetAllTask,
                mongoGetAllTask
            };
            await Task.WhenAll(taskList);

            IEnumerable<Product> products = MergeProducts(sqlGetAllTask.Result, mongoGetAllTask.Result);
            return products;
        }

        /// <summary>
        /// Asynchronously gets Product with the specific id
        /// </summary>
        /// <param name="id">Id of the Product to get</param>
        /// <returns>Task with specified Product</returns>
        public async Task<Product> GetAsync(string id)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(Guid.Parse(id));

            Task<Product> sqlGetTask = SqlProducts.GetAsync(id);
            string mongoId = GetMongoId(id);
            Task<Product> mongoGetTask = MongoProducts.GetAsync(mongoId);
            List<Task> taskList = new List<Task>
            {
                sqlGetTask,
                mongoGetTask
            };
            await Task.WhenAll(taskList);

            Product product = MergeProduct(sqlGetTask.Result, mongoGetTask.Result);
            return product;
        }

        /// <summary>
        /// Asynchronously adds Product
        /// </summary>
        /// <param name="product">Product to add</param>
        /// <returns>Product id</returns>
        public async Task<bool> CreateAsync(Product product)
        {
            EntityNameValidator validator = new EntityNameValidator();
            validator.ValidateAndThrow(product.Name);

            product.SetMongoId(MongoHelper.GenerateMongoId());
            Task<bool> sqlAddTask = SqlProducts.CreateAsync(product);
            List<Task<bool>> taskList = new List<Task<bool>> { sqlAddTask };
            if (product.HasMongoProperties())
            {
                taskList.Add(MongoProducts.CreateAsync(product));
            }
            await Task.WhenAll(taskList);
            return taskList.All(x => x.Result);
        }

        /// <summary>
        /// Asynchronously edits specified Product
        /// </summary>
        /// <param name="product">Product, that contains id of entity that should be changed, and all changed values</param>
        /// <returns>Operation success flag</returns>
        public async Task<bool> UpdateAsync(Product product)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(product.Id);

            bool sqlTaskNeeded = product.HasSqlProperties();
            bool mongoTaskNeeded = product.HasMongoProperties();
            List<Task> taskList = new List<Task>();
            if (sqlTaskNeeded)
            {
                taskList.Add(SqlProducts.UpdateAsync(product));
            }
            if (mongoTaskNeeded)
            {
                if (string.IsNullOrWhiteSpace(product.MongoId))
                {
                    product.SetMongoId(GetMongoId(product.Id.ToString()));
                }
                taskList.Add(MongoProducts.UpdateAsync(product));
            }
            await Task.WhenAll(taskList);

            if (sqlTaskNeeded)
            {
                Task<int> sqlTask = taskList.First(x => x is Task<int>) as Task<int>;
                int rowsAffected = sqlTask.Result;
                return rowsAffected == 1;
            }
            if (mongoTaskNeeded)
            {
                Product mongoProduct = await MongoProducts.GetAsync(product.MongoId);
                return CheckProductUpdated(product, mongoProduct);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Asynchronously removed Product with specified id
        /// </summary>
        /// <param name="id">Id of the Product to delete</param>
        /// <returns>Number of rows affected</returns>
        public async Task<bool> DeleteAsync(string id)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(Guid.Parse(id));

            Task<bool> sqlDeleteTask = SqlProducts.DeleteAsync(id);
            string mongoId = GetMongoId(id);
            Task<bool> mongoDeleteTask = MongoProducts.DeleteAsync(mongoId);
            List<Task<bool>> taskList = new List<Task<bool>>
            {
                sqlDeleteTask,
                mongoDeleteTask
            };
            await Task.WhenAll(taskList);
            return taskList.All(x => x.Result);
        }

        /// <summary>
        /// Compare two Products to have similar Mongo properties
        /// </summary>
        /// <param name="expected">Expected Product</param>
        /// <param name="actual">Actual Product</param>
        /// <returns>Comparison result</returns>
        private bool CheckProductUpdated(Product expected, Product actual)
        {
            if (expected.MongoId != actual.MongoId) return false;
            foreach (Models.Specifications.Specification spec in expected.Specifications)
            {
                if (actual.Specifications.Any(s => s.Name != spec.Name ||
                                              s.MaxSelectionAvailable != spec.MaxSelectionAvailable ||
                                              s.Options.Except(spec.Options).Any() ||
                                              spec.Options.Except(s.Options).Any())) return false;
            }
            return true;
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
