using GreenShop.Catalog.Api.Service.Categories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Api.Controllers
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
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategoriesAsync()
        {
            try
            {
                IEnumerable<CategoryDto> categories = await _categoriesService.GetAllAsync();
                if (categories == null) throw new ArgumentNullException();
                return Ok(categories);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        // GET api/categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategoryAsync(int id)
        {
            try
            {
                CategoryDto category = await _categoriesService.GetAsync(id);
                if (category == null) throw new ArgumentNullException();
                return Ok(category);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        // POST api/categories
        [HttpPost]
        public async Task<ActionResult<int>> AddCategoryAsync([FromBody] CategoryDto product)
        {
            try
            {
                int id = await _categoriesService.CreateAsync(product);
                return Ok(id);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // PUT api/categories/5
        [HttpPut]
        public async Task<ActionResult<bool>> EditCategoryAsync([FromBody] CategoryDto product)
        {
            try
            {
                bool success = await _categoriesService.UpdateAsync(product);
                return Ok(success);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // DELETE api/categories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteCategoryAsync(int id)
        {
            try
            {
                bool success = await _categoriesService.DeleteAsync(id);
                return Ok(success);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
