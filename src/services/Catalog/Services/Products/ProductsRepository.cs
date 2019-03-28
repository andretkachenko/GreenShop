using FluentValidation;
using GreenShop.Catalog.DataAccessors.Interfaces;
using GreenShop.Catalog.Extensions;
using GreenShop.Catalog.Helpers;
using GreenShop.Catalog.Models.Products;
using GreenShop.Catalog.Services.Products.Interfaces;
using GreenShop.Catalog.Validators;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Services.Products
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly ISqlDataAccessor<Product> SqlProducts;
        private readonly IMongoDataAccessor<Product> MongoProducts;
        private readonly IProductMerger ProductMerger;

        public ProductsRepository(ISqlDataAccessor<Product> sqlAccessor, IMongoDataAccessor<Product> mongoAccessor, IProductMerger productMerger)
        {
            SqlProducts = sqlAccessor;
            MongoProducts = mongoAccessor;
            ProductMerger = productMerger;
        }

        /// <summary>
        /// Asynchronously gets all Products
        /// </summary>
        /// <returns>Task with list of all Products</returns>
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            Task<IEnumerable<Product>> sqlGetAllTask = SqlProducts.GetAll();
            Task<IEnumerable<Product>> mongoGetAllTask = MongoProducts.GetAll();
            List<Task> taskList = new List<Task>
            {
                sqlGetAllTask,
                mongoGetAllTask
            };
            await Task.WhenAll(taskList);

            IEnumerable<Product> products = ProductMerger.MergeProducts(sqlGetAllTask.Result, mongoGetAllTask.Result);
            return products;
        }

        /// <summary>
        /// Asynchronously gets Product with the specific id
        /// </summary>
        /// <param name="id">Id of the Product to get</param>
        /// <returns>Task with specified Product</returns>
        public async Task<Product> GetProduct(int id)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(id);

            Task<Product> sqlGetTask = SqlProducts.Get(id);
            string mongoId = ProductMerger.GetMongoId(id);
            Task<Product> mongoGetTask = MongoProducts.Get(mongoId);
            List<Task> taskList = new List<Task>
            {
                sqlGetTask,
                mongoGetTask
            };
            await Task.WhenAll(taskList);

            Product product = ProductMerger.MergeProduct(sqlGetTask.Result, mongoGetTask.Result);
            return product;
        }

        /// <summary>
        /// Asynchronously adds Product
        /// </summary>
        /// <param name="product">Product to add</param>
        /// <returns>Product id</returns>
        public async Task<int> AddProduct(Product product)
        {
            EntityNameValidator validator = new EntityNameValidator();
            validator.ValidateAndThrow(product.Name);

            product.MongoId = MongoHelper.GenerateMongoId();
            Task<int> sqlAddTask = SqlProducts.Add(product);
            List<Task> taskList = new List<Task> { sqlAddTask };
            if (product.HasMongoProperties())
            {
                taskList.Add(MongoProducts.Add(product));
            }
            await Task.WhenAll(taskList);

            int id = sqlAddTask.Result;
            return id;
        }

        /// <summary>
        /// Asynchronously edits specified Product
        /// </summary>
        /// <param name="product">Product, that contains id of entity that should be changed, and all changed values</param>
        /// <returns>Operation success flag</returns>
        public async Task<bool> EditProduct(Product product)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(product.Id);

            bool sqlTaskNeeded = product.HasSqlProperties();
            bool mongoTaskNeeded = product.HasMongoProperties();
            List<Task> taskList = new List<Task>();
            if (sqlTaskNeeded)
            {
                taskList.Add(SqlProducts.Edit(product));
            }
            if (mongoTaskNeeded)
            {
                if (string.IsNullOrWhiteSpace(product.MongoId))
                {
                    product.MongoId = ProductMerger.GetMongoId(product.Id);
                }
                taskList.Add(MongoProducts.Edit(product));
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
                Product mongoProduct = await MongoProducts.Get(product.MongoId);
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
        public async Task<bool> DeleteProduct(int id)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(id);

            Task<int> sqlDeleteTask = SqlProducts.Delete(id);
            string mongoId = ProductMerger.GetMongoId(id);
            Task mongoDeleteTask = MongoProducts.Delete(mongoId);
            List<Task> taskList = new List<Task>
            {
                sqlDeleteTask,
                mongoDeleteTask
            };
            await Task.WhenAll(taskList);

            int rowsAffected = sqlDeleteTask.Result;
            bool success = rowsAffected == 1;
            return success;
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
    }
}
