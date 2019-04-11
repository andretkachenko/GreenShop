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
using GreenShop.Catalog.Domain.Products;
using GreenShop.Catalog.Properties;
using GreenShop.Catalog.Validators;
using MongoDB.Driver;
using GreenShop.Catalog.Models.Comments;

namespace GreenShop.Catalog.Service.Products
{
    public class ProductService
    {
        private ISqlProductRepository SqlProducts;
        private IMongoProductRepository MongoProducts;
        private ICommentRepository Comments;
        private ISqlContext SqlContext;
        private readonly IMongoContext MongoContext;
        private IMongoCollection<Product> MongoCollection => MongoContext.Database.GetCollection<Product>(Resources.Products);
        public IDbTransaction Transaction { get; private set; }

        public ProductService(ISqlContext sqlContext, 
            IMongoContext mongoContext,
            ISqlProductRepository sqlProducts,
            IMongoProductRepository mongoProducts,
            ICommentRepository comments)
        {
            SqlContext = sqlContext;
            MongoContext = mongoContext;
            SqlProducts = sqlProducts;
            MongoProducts = mongoProducts;
            Comments = comments;
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
            var commentsDict = await Comments.GetAllParentRelatedAsync(products.Select(x => x.Id));
            products.ToList().ForEach(x => x.Comments = commentsDict[x.Id]);

            return products;
        }

        /// <summary>
        /// Asynchronously gets Product with the specific id
        /// </summary>
        /// <param name="id">Id of the Product to get</param>
        /// <returns>Task with specified Product</returns>
        public async Task<Product> GetAsync(string id)
        {
            var guid = Guid.Parse(id);
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(guid);

            Task<Product> sqlGetTask = SqlProducts.GetAsync(id);
            string mongoId = RetrieveMongoIdFromSqlDb(id);
            Task<Product> mongoGetTask = MongoProducts.GetAsync(mongoId);
            Task<IEnumerable<Comment>> getCommentsTask = Comments.GetAllParentRelatedAsync(guid);
            List<Task> taskList = new List<Task>
            {
                sqlGetTask,
                mongoGetTask,
                getCommentsTask
            };
            await Task.WhenAll(taskList);

            Product product = MergeProduct(sqlGetTask.Result, mongoGetTask.Result);
            product.Comments = getCommentsTask.Result;
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
            if((bool)product.Comments?.Any())
            {
                taskList.Add(Comments.CreateAsync(product.Comments));
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
            List<Task<bool>> taskList = new List<Task<bool>>();
            if (sqlTaskNeeded)
            {
                taskList.Add(SqlProducts.UpdateAsync(product));
            }
            if (mongoTaskNeeded)
            {
                if (string.IsNullOrWhiteSpace(product.MongoId))
                {
                    var mongoId = RetrieveMongoIdFromSqlDb(product.Id.ToString());
                    product.SetMongoId(mongoId);
                }
                taskList.Add(MongoProducts.UpdateAsync(product));
            }
            await Task.WhenAll(taskList);

            return taskList.All(x => x.Result);
        }

        /// <summary>
        /// Asynchronously removed Product with specified id
        /// </summary>
        /// <param name="id">Id of the Product to delete</param>
        /// <returns>Number of rows affected</returns>
        public async Task<bool> DeleteAsync(string id)
        {
            Guid guid = Guid.Parse(id);
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(guid);

            Task<bool> sqlDeleteTask = SqlProducts.DeleteAsync(id);
            string mongoId = RetrieveMongoIdFromSqlDb(id);
            Task<bool> mongoDeleteTask = MongoProducts.DeleteAsync(mongoId);
            Task<bool> deleteCommentsTask = Comments.DeleteAllParentRelatedAsync(guid);
            List<Task<bool>> taskList = new List<Task<bool>>
            {
                sqlDeleteTask,
                mongoDeleteTask,
                deleteCommentsTask
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
        private string RetrieveMongoIdFromSqlDb(string id)
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
