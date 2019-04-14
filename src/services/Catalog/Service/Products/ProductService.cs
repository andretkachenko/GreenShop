using AutoMapper;
using Dapper;
using FluentValidation;
using GreenShop.Catalog.Config.Interfaces;
using GreenShop.Catalog.Domain.Products;
using GreenShop.Catalog.Helpers;
using GreenShop.Catalog.Infrastructure;
using GreenShop.Catalog.Properties;
using GreenShop.Catalog.Validators;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Service.Products
{
    public class ProductService : IProductService
    {
        private IDomainScope Scope;
        private ISqlContext SqlContext;
        private readonly IMongoContext MongoContext;
        private IMongoCollection<Product> MongoCollection => MongoContext.Database.GetCollection<Product>(Resources.Products);
        public IDbTransaction Transaction { get; private set; }

        public ProductService(ISqlContext sqlContext,
            IMongoContext mongoContext,
            IDomainScope unitOfWork)
        {
            SqlContext = sqlContext;
            MongoContext = mongoContext;
            Scope = unitOfWork;
        }

        /// <summary>
        /// Asynchronously gets all Products
        /// </summary>
        /// <returns>Task with list of all Products</returns>
        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            using (Scope)
            {
                Task<IEnumerable<Product>> sqlGetAllTask = Scope.SqlProducts.GetAllAsync();
                Task<IEnumerable<Product>> mongoGetAllTask = Scope.MongoProducts.GetAllAsync();
                List<Task> taskList = new List<Task>
                {
                    sqlGetAllTask,
                    mongoGetAllTask
                };
                await Task.WhenAll(taskList);
                IEnumerable<Product> products = MergeProducts(sqlGetAllTask.Result, mongoGetAllTask.Result);
                Dictionary<Guid, IEnumerable<Comment>> commentsDict = await Scope.Comments.GetAllParentRelatedAsync(products.Select(x => x.Id));
                products.ToList().ForEach(x => x.Comments = commentsDict[x.Id]);

                IEnumerable<ProductDto> result = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);
                return result;
            }
        }

        /// <summary>
        /// Asynchronously gets Product with the specific id
        /// </summary>
        /// <param name="id">Id of the Product to get</param>
        /// <returns>Task with specified Product</returns>
        public async Task<ProductDto> GetAsync(string id)
        {
            Guid guid = Guid.Parse(id);
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(guid);

            using (Scope)
            {
                Task<Product> sqlGetTask = Scope.SqlProducts.GetAsync(id);
                string mongoId = RetrieveMongoIdFromSqlDb(id);
                Task<Product> mongoGetTask = Scope.MongoProducts.GetAsync(mongoId);
                Task<IEnumerable<Comment>> getCommentsTask = Scope.Comments.GetAllParentRelatedAsync(guid);
                List<Task> taskList = new List<Task>
                {
                    sqlGetTask,
                    mongoGetTask,
                    getCommentsTask
                };
                await Task.WhenAll(taskList);

                Product product = MergeProduct(sqlGetTask.Result, mongoGetTask.Result);
                product.Comments = getCommentsTask.Result;

                ProductDto result = Mapper.Map<Product, ProductDto>(product);
                return result;
            }
        }

        /// <summary>
        /// Asynchronously adds Product
        /// </summary>
        /// <param name="productDto">Product to add</param>
        /// <returns>Product id</returns>
        public async Task<Guid> CreateAsync(ProductDto productDto)
        {
            EntityNameValidator validator = new EntityNameValidator();
            validator.ValidateAndThrow(productDto.Name);

            using (Scope)
            {
                Product product = new Product(productDto.Name, productDto.CategoryId, productDto.Description);
                product.UpdateBasePrice(productDto.BasePrice);
                product.UpdateRating(productDto.Rating);
                product.SetMongoId(MongoHelper.GenerateMongoId());

                List<Specification> specs = new List<Specification>();
                foreach (SpecificationDto specDto in productDto.Specifications)
                {
                    specs.Add(new Specification(specDto.Name, specDto.MaxSelectionAvailable, specDto.Options));
                }
                product.UpdateSpecifications(specs);

                foreach (CommentDto commentDto in productDto.Comments)
                {
                    Comment comment = new Comment(commentDto.AuthorId, commentDto.Message, commentDto.ProductId);
                    product.AddComment(comment);
                }

                try
                {
                    Scope.Begin();

                    Task<bool> sqlAddTask = Scope.SqlProducts.CreateAsync(product);
                    List<Task<bool>> taskList = new List<Task<bool>> { sqlAddTask };
                    if (product.HasMongoProperties())
                    {
                        taskList.Add(Scope.MongoProducts.CreateAsync(product));
                    }
                    if ((bool)productDto.Comments?.Any())
                    {
                        taskList.Add(Scope.Comments.CreateAsync(product.Comments));
                    }
                    await Task.WhenAll(taskList);

                    Scope.Commit();

                    return product.Id;
                }
                catch (Exception e)
                {
                    Scope.Rollback();
                    throw e;
                }
            }
        }

        /// <summary>
        /// Asynchronously edits specified Product
        /// </summary>
        /// <param name="productDto">Product, that contains id of entity that should be changed, and all changed values</param>
        /// <returns>Operation success flag</returns>
        public async Task<bool> UpdateAsync(ProductDto productDto)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(productDto.Id);

            using (Scope)
            {
                Product product = new Product(productDto.Name, productDto.CategoryId, productDto.Description);
                product.UpdateBasePrice(productDto.BasePrice);
                product.UpdateRating(productDto.Rating);
                product.SetMongoId(MongoHelper.GenerateMongoId());

                List<Specification> specs = new List<Specification>();
                foreach (SpecificationDto specDto in productDto.Specifications)
                {
                    specs.Add(new Specification(specDto.Name, specDto.MaxSelectionAvailable, specDto.Options));
                }
                product.UpdateSpecifications(specs);

                foreach (CommentDto commentDto in productDto.Comments)
                {
                    Comment comment = new Comment(commentDto.AuthorId, commentDto.Message, commentDto.ProductId);
                    product.AddComment(comment);
                }

                bool sqlTaskNeeded = product.HasSqlProperties();
                bool mongoTaskNeeded = product.HasMongoProperties();

                try
                {
                    Scope.Begin();

                    List<Task<bool>> taskList = new List<Task<bool>>();
                    if (sqlTaskNeeded)
                    {
                        taskList.Add(Scope.SqlProducts.UpdateAsync(product));
                    }
                    if (mongoTaskNeeded)
                    {
                        if (string.IsNullOrWhiteSpace(product.MongoId))
                        {
                            string mongoId = RetrieveMongoIdFromSqlDb(product.Id.ToString());
                            product.SetMongoId(mongoId);
                        }
                        taskList.Add(Scope.MongoProducts.UpdateAsync(product));
                    }
                    await Task.WhenAll(taskList);

                    Scope.Commit();

                    return taskList.All(x => x.Result);
                }
                catch (Exception e)
                {
                    Scope.Rollback();
                    throw e;
                }
            }
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

            using (Scope)
            {
                try
                {
                    Scope.Begin();

                    Task<bool> sqlDeleteTask = Scope.SqlProducts.DeleteAsync(id);
                    string mongoId = RetrieveMongoIdFromSqlDb(id);
                    Task<bool> mongoDeleteTask = Scope.MongoProducts.DeleteAsync(mongoId);
                    Task<bool> deleteCommentsTask = Scope.Comments.DeleteAllParentRelatedAsync(guid);
                    List<Task<bool>> taskList = new List<Task<bool>>
                    {
                        sqlDeleteTask,
                        mongoDeleteTask,
                        deleteCommentsTask
                    };
                    await Task.WhenAll(taskList);

                    Scope.Commit();
                    return taskList.All(x => x.Result);
                }
                catch (Exception e)
                {
                    Scope.Rollback();
                    throw e;
                }
            }
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
            foreach (Specification spec in expected.Specifications)
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
