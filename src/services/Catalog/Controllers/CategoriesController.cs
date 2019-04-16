using GreenShop.Catalog.Service.Categories;
using Microsoft.AspNetCore.Mvc;
using System;
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
        private readonly ICategoryService _categoriesService;

        public CategoriesController(ICategoryService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        // GET api/categories
        [HttpGet]
        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            IEnumerable<CategoryDto> categories = await _categoriesService.GetAllAsync();

            return categories;
        }

        // GET api/categories/5
        [HttpGet("{id}")]
        public async Task<CategoryDto> GetCategoryAsync(string id)
        {
            CategoryDto category = await _categoriesService.GetAsync(id);

            return category;
        }

        // POST api/categories
        [HttpPost]
        public async Task<Guid> AddCategoryAsync([FromBody] CategoryDto category)
        {
            Guid id = await _categoriesService.CreateAsync(category);

            return id;
        }

        // PUT api/categories/5
        [HttpPut]
        public async Task<bool> EditCategoryAsync([FromBody] CategoryDto category)
        {
            bool success = await _categoriesService.UpdateAsync(category);

            return success;
        }

        // DELETE api/categories/5
        [HttpDelete("{id}")]
        public async Task<bool> DeleteCategoryAsync(string id)
        {
            bool success = await _categoriesService.DeleteAsync(id);

            return success;
        }
    }
}
