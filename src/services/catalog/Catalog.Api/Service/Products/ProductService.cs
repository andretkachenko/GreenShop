using AutoMapper;
using FluentValidation;
using GreenShop.Catalog.Api.Domain.Products;
using GreenShop.Catalog.Api.Helpers;
using GreenShop.Catalog.Api.Infrastructure;
using GreenShop.Catalog.Api.Properties;
using GreenShop.Catalog.Api.Validators;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Api.Service.Products
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly IDomainScope Scope;

        public ProductService(IMapper mapper, IDomainScope unitOfWork)
        {
            _mapper = mapper;
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
                Dictionary<int, IEnumerable<Comment>> commentsDict = await Scope.Comments.GetAllParentRelatedAsync(products.Select(x => x.Id));

                foreach(var product in products)
                {
                    if(commentsDict.ContainsKey(product.Id))
                    {
                        product.Comments = commentsDict[product.Id]?.ToList();
                    }                    
                }

                IEnumerable<ProductDto> result = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);
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
                product.Comments = getCommentsTask.Result?.ToList();

                ProductDto result = _mapper.Map<Product, ProductDto>(product);
                return result;
            }
        }

        /// <summary>
        /// Asynchronously adds Product
        /// </summary>
        /// <param name="productDto">Product to add</param>
        /// <returns>Product id</returns>
        public async Task<int> CreateAsync(ProductDto productDto)
        {
            EntityNameValidator validator = new EntityNameValidator();
            validator.ValidateAndThrow(productDto.Name);

            using (Scope)
            {
                Product product = new Product(productDto.Name, productDto.CategoryId, productDto.Description);
                product.UpdateBasePrice(productDto.BasePrice);
                product.UpdateRating(productDto.Rating);
                product.SetMongoId(MongoHelper.GenerateMongoId());

                AddSpecificationsFromDtoList(product, productDto.Specifications);
                AddCommentsFromDtoList(product, productDto.Comments);

                try
                {
                    Scope.Begin();

                    Task<int> sqlAddTask = Scope.SqlProductRepository.CreateAsync(product);
                    List<Task> taskList = new List<Task> { sqlAddTask };
                    if (product.HasMongoProperties())
                    {
                        taskList.Add(Scope.MongoProductRepository.CreateAsync(product));
                    }
                    if (product.Comments != null && product.Comments.Count() > 0)
                    {
                        taskList.Add(Scope.Comments.CreateAsync(product.Comments));
                    }
                    await Task.WhenAll(taskList);

                    Scope.Commit();

                    return sqlAddTask.Result;
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

                AddSpecificationsFromDtoList(product, productDto.Specifications);
                AddCommentsFromDtoList(product, productDto.Comments);

                bool sqlTaskNeeded = product.HasSqlProperties();
                bool mongoTaskNeeded = product.HasMongoProperties();

                try
                {
                    Scope.Begin();

                    List<Task<bool>> taskList = new List<Task<bool>>();
                    if (sqlTaskNeeded)
                    {
                        taskList.Add(Scope.SqlProductRepository.UpdateAsync(productDto));
                    }
                    if (mongoTaskNeeded)
                    {
                        if (string.IsNullOrWhiteSpace(product.MongoId))
                        {
                            string mongoId = await Scope.SqlProductRepository.GetMongoIdAsync(product.Id);
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
        public async Task<bool> DeleteAsync(int id)
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
        public async Task<int> AddCommentAsync(CommentDto commentDto)
        {
            CommentValidator validator = new CommentValidator();
            validator.ValidateAndThrow(commentDto);

            using (Scope)
            {
                try
                {
                    Scope.Begin();
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
        }

        /// <summary>
        /// Asynchronously Deletes Comment by Id
        /// </summary>
        /// <param name="id">Id of the Comment to delete</param>
        /// <returns>True if succeeded</returns>
        public async Task<bool> DeleteCommentAsync(int id)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(id);

            using (Scope)
            {
                try
                {
                    Scope.Begin();
                    bool result = await Scope.Comments.DeleteAsync(id);
                    Scope.Commit();
                    return result;
                }
                catch (Exception e)
                {
                    Scope.Rollback();
                    throw e;
                }
            }
        }

        /// <summary>
        /// Asynchronously Edit comment's message
        /// </summary>
        /// <param name="id">Id of the Comment to edit</param>
        /// <param name="message">Updated message for the comment</param>
        /// <returns>True if succeeded</returns>
        public async Task<bool> EditCommentAsync(int id, string message)
        {
            IdValidator idValidator = new IdValidator();
            idValidator.ValidateAndThrow(id);
            CommentMessageValidator messageValidator = new CommentMessageValidator();
            messageValidator.ValidateAndThrow(message);

            using (Scope)
            {
                try
                {
                    Scope.Begin();
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

        private void AddCommentsFromDtoList(Product product, IEnumerable<CommentDto> commentDtos)
        {
            if (commentDtos != null && commentDtos.Count() > 0)
            {
                foreach (CommentDto commentDto in commentDtos)
                {
                    product.AddComment(commentDto.AuthorId, commentDto.Message);
                }
            }
        }

        private void AddSpecificationsFromDtoList(Product product, IEnumerable<SpecificationDto> specificationDtos)
        {
            if (specificationDtos != null && specificationDtos.Count() > 0)
            {
                List<Specification> specs = new List<Specification>();
                foreach (SpecificationDto specDto in specificationDtos)
                {
                    specs.Add(new Specification(specDto.Name, specDto.MaxSelectionAvailable, specDto.Options));
                }
                product.UpdateSpecifications(specs);
            }
        }
    }
}
