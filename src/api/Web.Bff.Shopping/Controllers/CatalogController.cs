using Web.Bff.Shopping.Services.Catalog.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Common.Models.Categories;

namespace Web.Bff.Shopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;

        public CatalogController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
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
    }
}
