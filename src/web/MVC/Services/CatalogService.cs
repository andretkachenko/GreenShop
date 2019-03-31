using FluentValidation;
using GreenShop.MVC.Models.Categories;
using GreenShop.MVC.Models.Comments;
using GreenShop.MVC.Models.DTO;
using GreenShop.MVC.Models.Products;
using GreenShop.MVC.Services.Interfaces;
using GreenShop.MVC.Validators;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenShop.MVC.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly ICatalogConsumer _catalogConsumer;

        public CatalogService(ICatalogConsumer catalogConsumer)
        {
            _catalogConsumer = catalogConsumer;
        }

        /// <summary>
        /// Asynchronously adds Category
        /// </summary>
        /// <param name="category">Category to add</param>
        /// <returns>Task with Category id</returns>
        public async Task<int> AddCategoryAsync(Category category)
        {
            EntityNameValidator validator = new EntityNameValidator();
            validator.ValidateAndThrow(category.Name);

            int id = await _catalogConsumer.AddCategoryAsync(category);

            return id;
        }

        /// <summary>
        /// Asynchronously add Product
        /// </summary>
        /// <param name="Product">Product to add</param>
        /// <returns>Task with Product id</returns>
        public async Task<int> AddProductAsync(Product product)
        {
            EntityNameValidator validator = new EntityNameValidator();
            validator.ValidateAndThrow(product.Name);

            int id = await _catalogConsumer.AddProductAsync(product);
            return id;
        }

        /// <summary>
        /// Asynchronously removed Category with specified id
        /// </summary>
        /// <param name="id">Id of the Category to delete</param>
        /// <returns>Task with success flag</returns>
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(id);

            bool success = await _catalogConsumer.DeleteCategoryAsync(id);

            return success;
        }

        /// <summary>
        /// Asynchronously remove Product with specified id
        /// </summary>
        /// <param name="id">Id of the Product to delete</param>
        /// <returns>Task with success flag</returns>
        public async Task<bool> DeleteProductAsync(int id)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(id);

            bool success = await _catalogConsumer.DeleteProductAsync(id);

            return success;
        }

        /// <summary>
        /// Asynchronously edits specified Category
        /// </summary>
        /// <param name="category">Category, that contains id of entity that should be changed, and all changed values</param>
        /// <returns>Task with success flag</returns>
        public async Task<bool> EditCategoryAsync(Category category)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(category.Id);

            bool success = await _catalogConsumer.EditCategoryAsync(category);

            return success;
        }

        /// <summary>
        /// Asynchronously edit specified Product
        /// </summary>
        /// <param name="Product">Product, that contains id of entity that should be changed, and all changed values</param>
        /// <returns>Task with success flag</returns>
        public async Task<bool> EditProductAsync(Product product)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(product.Id);

            bool success = await _catalogConsumer.EditProductAsync(product);

            return success;
        }

        /// <summary>
        /// Asynchronously gets all Categories
        /// </summary>
        /// <returns>Task with list of all Categories</returns>
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            IEnumerable<Category> categories = await _catalogConsumer.GetAllCategoriesAsync();

            return categories;
        }

        /// <summary>
        /// Asynchronously get all Products
        /// </summary>
        /// <returns>Task with list of all Products</returns>
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            IEnumerable<Product> products = await _catalogConsumer.GetAllProductsAsync();

            return products;
        }

        /// <summary>
        /// Asynchronously gets Category with the specific id
        /// </summary>
        /// <param name="id">Id of the Category to get</param>
        /// <returns>Task with specified Category</returns>
        public async Task<Category> GetCategoryAsync(int id)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(id);

            Category category = await _catalogConsumer.GetCategoryAsync(id);

            return category;
        }

        /// <summary>
        /// Asynchronously get Category with the specific id and Products which are connected to the specified Category
        /// </summary>
        /// <param name="id">Id of the Category to get</param>
        /// <returns>Specified Category and list of related Products</returns>
        public async Task<CategoryProductsDTO> GetCategoryWithProductsAsync(int id)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(id);

            CategoryProductsDTO result = await _catalogConsumer.GetCategoryWithProductsAsync(id);

            return result;
        }

        /// <summary>
        /// Asynchronously get Product with the specific id
        /// </summary>
        /// <param name="id">Id of the Product to get</param>
        /// <returns>Task with specified Product</returns>
        public async Task<Product> GetProductAsync(int id)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(id);

            Product product = await _catalogConsumer.GetProductAsync(id);

            return product;
        }

        /// <summary>
        /// Asynchronously get Product with the specific id with the related Category
        /// </summary>
        /// <param name="id">Id of the Product to get</param>
        /// <returns>Specified Product with the related Category</returns>
        public async Task<Product> GetProductWithCategoryAsync(int id)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(id);

            Product result = await _catalogConsumer.GetProductWithCategoryAsync(id);

            return result;
        }

        #region Comment

        /// <summary>
        /// Asynchronously get all product related comments by product Id 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>Collection of comments</returns>
        public async Task<IEnumerable<Comment>> GetAllProductComments(int productId)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(productId);

            IEnumerable<Comment> comments = await _catalogConsumer.GetallProductCommentsAsync(productId);

            return comments;
        }

        /// <summary>
        /// Asynchronously get Comment by specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Comment</returns>
        public async Task<Comment> GetCommentAsync(int id)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(id);

            Comment comment = await _catalogConsumer.GetCommentAsync(id);

            return comment;
        }

        /// <summary>
        /// Asynchronously add Comment
        /// </summary>
        /// <param name="comment"></param>
        /// <returns>Comment id</returns>
        public async Task<int> AddCommentAsync(Comment comment)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(comment.ProductId);
            EntityNameValidator stringValidator = new EntityNameValidator();
            stringValidator.ValidateAndThrow(comment.Message);

            int id = await _catalogConsumer.AddCommentAsync(comment);

            return id;
        }

        /// <summary>
        /// Asynchronously edit specified Comment
        /// </summary>
        /// <param name="comment"></param>
        /// <returns>Boolean result</returns>
        public async Task<bool> EditCommentAsync(Comment comment)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(comment.Id);
            EntityNameValidator stringValidator = new EntityNameValidator();
            stringValidator.ValidateAndThrow(comment.Message);

            bool result = await _catalogConsumer.EditCommentAsync(comment.Id, comment.Message);

            return result;
        }

        /// <summary>
        /// Asynchronously delete Comment by specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Boolean result</returns>
        public async Task<bool> DeleteCommentAsync(int id)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(id);

            bool result = await _catalogConsumer.DeleteCommentAsync(id);

            return result;
        }

        #endregion
    }
}
