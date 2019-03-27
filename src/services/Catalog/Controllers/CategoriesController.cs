using GreenShop.Catalog.Services.Categories.Interfaces;
using Common.Models.Categories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Controllers
{
    [ApiVersion("1")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesRepository _categoriesService;

        public CategoriesController(ICategoriesRepository categoriesService)
        {
            _categoriesService = categoriesService;
        }

        // GET api/categories
        [HttpGet]
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            IEnumerable<Category> categories = await _categoriesService.GetAllCategories();

            return categories;
        }

        // GET api/categories/5
        [HttpGet("{id}")]
        public async Task<Category> GetCategoryAsync(int id)
        {
            Category category = await _categoriesService.GetCategory(id);

            return category;
        }

        // POST api/categories
        [HttpPost]
        public async Task<int> AddCategoryAsync([FromBody] Category category)
        {
            int success = await _categoriesService.AddCategory(category);

            return success;
        }

        // PUT api/categories/5
        [HttpPut]
        public async Task<bool> EditCategoryAsync([FromBody] Category category)
        {
            bool success = await _categoriesService.EditCategory(category);

            return success;
        }

        // DELETE api/categories/5
        [HttpDelete("{id}")]
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            bool success = await _categoriesService.DeleteCategory(id);

            return success;
        }
    }
}
