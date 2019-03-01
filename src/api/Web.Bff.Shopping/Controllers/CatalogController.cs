using Common.Models.Categories;
using Common.Models.Comments;
using Common.Models.Products;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Bff.Shopping.Services.Catalog.Interfaces;

namespace Web.Bff.Shopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;
        private readonly IProductsService _productsService;
        private readonly ICommentService _commentService;

        public CatalogController(ICategoriesService categoriesService,
                                 IProductsService productsService,
                                 ICommentService commentService)
        {
            _categoriesService = categoriesService;
            _productsService = productsService;
            _commentService = commentService;
        }

        #region Categories
        #region CRUD
        // GET api/catalog/categories
        [HttpGet("categories")]
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            var categories = await _categoriesService.GetAllCategories();

            return categories;
        }

        // GET api/catalog/categories/5
        [HttpGet("categories/{id}")]
        public async Task<Category> GetCategoryAsync(int id)
        {
            var category = await _categoriesService.GetCategory(id);

            return category;
        }

        // POST api/catalog/categories
        [HttpPost("categories")]
        public async Task<int> AddCategoryAsync([FromBody] Category category)
        {
            var success = await _categoriesService.AddCategory(category);

            return success;
        }

        // PUT api/catalog/categories/5
        [HttpPut("categories")]
        public async Task<bool> EditCategoryAsync([FromBody] Category category)
        {
            var success = await _categoriesService.EditCategory(category);

            return success;
        }

        // DELETE api/catalog/categories/5
        [HttpDelete("categories/{id}")]
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var success = await _categoriesService.DeleteCategory(id);

            return success;
        }
        #endregion
        #endregion

        #region Products
        #region CRUD
        // GET api/catalog/products
        [HttpGet("products")]
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var products = await _productsService.GetAllProducts();

            return products;
        }

        // GET api/catalog/products/5
        [HttpGet("products/{id}")]
        public async Task<Product> GetProductAsync(int id)
        {
            var product = await _productsService.GetProduct(id);

            return product;
        }

        // POST api/catalog/products
        [HttpPost("products")]
        public async Task<int> AddProductAsync([FromBody] Product product)
        {
            var success = await _productsService.AddProduct(product);

            return success;
        }

        // PUT api/catalog/products/5
        [HttpPut("products")]
        public async Task<bool> EditProductAsync([FromBody] Product product)
        {
            var success = await _productsService.EditProduct(product);

            return success;
        }

        // DELETE api/catalog/products/5
        [HttpDelete("products/{id}")]
        public async Task<bool> DeleteProductAsync(int id)
        {
            var success = await _productsService.DeleteProduct(id);

            return success;
        }
        #endregion
        #endregion

        #region Comments

        #region CRUD
        // GET api/catalog/comments/1
        [HttpGet("comments/{id}")]
        public async Task<Comment> GetComment(int id)
        {
            var result = await _commentService.GetComment(id);

            return result;
        }

        // GET api/catalog/comments/products/2
        [HttpGet("comments/products/{id}")]
        public async Task<IEnumerable<Comment>> GetProductRelatedComment(int id)
        {
            var result = await _commentService.GetAllProductComments(id);

            return result;
        }

        // PUT api/catalog/comments/1
        [HttpPut("comments/{id}")]
        public async Task<bool> EditComment(int id, [FromBody] string message)
        {
            var result = await _commentService.EditComment(id, message);

            return result;
        }

        //Delete api/catalog/comments/1
        [HttpDelete("comments/{id}")]
        public async Task<bool> DeleteComment(int id)
        {
            var result = await _commentService.DeleteComment(id);

            return result;
        }

        //Post api/catalog/comments/1
        [HttpPost("comments")]
        public async Task<int> AddComment([FromBody] Comment comment)
        {
            var result = await _commentService.AddComment(comment);

            return result;
        }

        #endregion

        #endregion
    }
}
