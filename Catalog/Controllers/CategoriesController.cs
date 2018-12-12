using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Services.Categories.Interfaces;
using Common.Models.Categories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;
        
        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        // GET api/categories
        [HttpGet]
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            var categories = await _categoriesService.GetAllCategories();

            return categories;
        }

        // GET api/categories/5
        [HttpGet("{id}")]
        public async Task<Category> GetCategoryAsync(int id)
        {
            var category = await _categoriesService.GetCategory(id);

            return category;
        }

        // POST api/categories
        [HttpPost]
        public async Task<bool> AddCategoryAsync([FromBody] Category category)
        {
            var success = await _categoriesService.AddCategory(category);

            return success;
        }

        // PUT api/categories/5
        [HttpPut]
        public async Task<bool> EditCategoryAsync([FromBody] Category category)
        {
            var success = await _categoriesService.EditCategory(category);

            return success;
        }

        // DELETE api/categories/5
        [HttpDelete("{id}")]
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var success = await _categoriesService.DeleteCategory(id);

            return success;
        }
    }
}
