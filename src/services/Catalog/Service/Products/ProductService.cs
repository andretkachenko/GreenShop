using AutoMapper;
using FluentValidation;
using GreenShop.Catalog.Domain.Products;
using GreenShop.Catalog.Helpers;
using GreenShop.Catalog.Infrastructure;
using GreenShop.Catalog.Properties;
using GreenShop.Catalog.Validators;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Service.Products
{
    public class ProductService : IProductService
    {
        private IDomainScope Scope;

        public ProductService(IDomainScope unitOfWork)
        {
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
                Task<IEnumerable<Product>> sqlGetAllTask = Scope.SqlProductRepository.GetAllAsync();
                Task<IEnumerable<Product>> mongoGetAllTask = Scope.MongoProductRepository.GetAllAsync();
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
        public async Task<ProductDto> GetAsync(int id)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(id);

            using (Scope)
            {
                Task<Product> sqlGetTask = Scope.SqlProductRepository.GetAsync(id);
                Task<string> getMongoIdTask = Scope.SqlProductRepository.GetMongoIdAsync(id);
                Task<Product> mongoGetTask = getMongoIdTask.ContinueWith(x => Scope.MongoProductRepository.GetAsync(x.Result)).Unwrap();
                Task<IEnumerable<Comment>> getCommentsTask = Scope.Comments.GetAllParentRelatedAsync(id);
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

                    Task<bool> sqlAddTask = Scope.SqlProductRepository.CreateAsync(product);
                    List<Task<bool>> taskList = new List<Task<bool>> { sqlAddTask };
                    if (product.HasMongoProperties())
                    {
                        taskList.Add(Scope.MongoProductRepository.CreateAsync(product));
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
                        taskList.Add(Scope.SqlProductRepository.UpdateAsync(product));
                    }
                    if (mongoTaskNeeded)
                    {
                        if (string.IsNullOrWhiteSpace(product.MongoId))
                        {
                            string mongoId = await Scope.SqlProductRepository.GetMongoIdAsync(product.Id.ToString());
                            product.SetMongoId(mongoId);
                        }
                        taskList.Add(Scope.MongoProductRepository.UpdateAsync(product));
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
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(id);

            using (Scope)
            {
                try
                {
                    Scope.Begin();

                    Task<bool> sqlDeleteTask = Scope.SqlProductRepository.DeleteAsync(id);
                    Task<string> getMongoIdTask = Scope.SqlProductRepository.GetMongoIdAsync(id);
                    Task<bool> mongoDeleteTask = getMongoIdTask.ContinueWith(x => Scope.MongoProductRepository.DeleteAsync(x.Result)).Unwrap();
                    Task<bool> deleteCommentsTask = Scope.Comments.DeleteAllParentRelatedAsync(id);
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
        ///Asynchronously adds Comment 
        /// </summary>
        /// <param name="commentDto">Comment to add</param>
        /// <returns>True if succeeded</returns>
        public async Task<Guid> AddCommentAsync(CommentDto commentDto)
        {
            CommentValidator validator = new CommentValidator();
            validator.ValidateAndThrow(commentDto);

            Scope.Begin();
            try
            {
                Comment comment = new Comment(commentDto.AuthorId, commentDto.Message, commentDto.ProductId);
                await Scope.Comments.CreateAsync(comment);
                Scope.Commit();
                return comment.Id;
            }
            catch (Exception e)
            {
                Scope.Rollback();
                throw e;
            }
        }

        /// <summary>
        /// Asynchronously Deletes Comment by Id
        /// </summary>
        /// <param name="id">Id of the Comment to delete</param>
        /// <returns>True if succeeded</returns>
        public async Task<bool> DeleteCommentAsync(Guid id)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(id);
            Scope.Begin();
            try
            {
                bool result = await Scope.Comments.DeleteAsync(id.ToString());
                Scope.Commit();
                return result;
            }
            catch (Exception e)
            {
                Scope.Rollback();
                throw e;
            }
        }

        /// <summary>
        /// Asynchronously Edit comment's message
        /// </summary>
        /// <param name="id">Id of the Comment to edit</param>
        /// <param name="message">Updated message for the comment</param>
        /// <returns>True if succeeded</returns>
        public async Task<bool> EditComment(Guid id, string message)
        {
            IdValidator idValidator = new IdValidator();
            idValidator.ValidateAndThrow(id);
            CommentMessageValidator messageValidator = new CommentMessageValidator();
            messageValidator.ValidateAndThrow(message);
            Scope.Begin();
            try
            {
                bool result = await Scope.Comments.UpdateAsync(id, message);
                Scope.Commit();
                return result;
            }
            catch (Exception e)
            {
                Scope.Rollback();
                throw e;
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
