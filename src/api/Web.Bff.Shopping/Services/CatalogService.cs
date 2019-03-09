using Common.Models.Categories;
using Common.Models.DTO;
using Common.Models.Products;
using Common.Validatiors;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Bff.Shopping.Services.Catalog.Interfaces;
using Web.Bff.Shopping.Services.Interfaces;

namespace Web.Bff.Shopping.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly IConsumer<Category> _categoriesConsumer;
        private readonly IConsumer<Product> _productsConsumer;

        public CatalogService(IConsumer<Category> categoriesConsumer,
                              IConsumer<Product> productsConsumer)
        {
            _categoriesConsumer = categoriesConsumer;
            _productsConsumer = productsConsumer;
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

            int id = await _categoriesConsumer.AddAsync(category);

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

            int id = await _productsConsumer.AddAsync(product);
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

            bool success = await _categoriesConsumer.DeleteAsync(id);

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

            bool success = await _productsConsumer.DeleteAsync(id);

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

            bool success = await _categoriesConsumer.EditAsync(category);

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

            bool success = await _productsConsumer.EditAsync(product);

            return success;
        }

        /// <summary>
        /// Asynchronously gets all Categories
        /// </summary>
        /// <returns>Task with list of all Categories</returns>
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            IEnumerable<Category> categories = await _categoriesConsumer.GetAllAsync();

            return categories;
        }

        /// <summary>
        /// Asynchronously get all Products
        /// </summary>
        /// <returns>Task with list of all Products</returns>
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            IEnumerable<Product> products = await _productsConsumer.GetAllAsync();

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

            Category category = await _categoriesConsumer.GetAsync(id);

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

            Task<Category> getCategoryTask = _categoriesConsumer.GetAsync(id);
            Task<IEnumerable<Product>> getAllProductsTask = _productsConsumer.GetAllAsync();
            List<Task> taskList = new List<Task>
            {
                getCategoryTask,
                getAllProductsTask
            };

            await Task.WhenAll(taskList);

            Category category = getCategoryTask.Result;
            List<Product> relatedProducts = getAllProductsTask.Result.Where(product => product.CategoryId == id).ToList();

            CategoryProductsDTO dto = new CategoryProductsDTO
            {
                Category = category,
                Products = relatedProducts
            };

            return dto;
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

            Product product = await _productsConsumer.GetAsync(id);

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

            Product product = await _productsConsumer.GetAsync(id);
            product.Category = await _categoriesConsumer.GetAsync(product.CategoryId);

            return product;
        }
    }
}
