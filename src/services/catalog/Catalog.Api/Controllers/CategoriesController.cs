using GreenShop.Catalog.Api.Properties;
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

        /// <summary>
        /// Retrieve all Categories
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/categories
        ///     
        /// </remarks>
        /// <returns>List of Categories, which are presented in the system</returns>
        /// <response code="200">Return the list of all Categories</response>
        /// <response code="404">None of the Categories are not presented in the system</response>  
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
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

        /// <summary>
        /// Retrieve Category with the specified Id
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET api/categories/5
        ///     
        /// </remarks>
        /// <returns>Category with the specified Id</returns>
        /// <response code="200">Return the Category</response>
        /// <response code="404">Category with the specified Id was not found</response>
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [Produces(typeof(CategoryDto))]
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

        /// <summary>
        /// Create a Category
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/categories
        ///     {
        ///     	"name": "SampleSubCatName",
        ///     	"parentCategoryId": 2
        ///     }
        /// 
        /// </remarks>
        /// <param name="category">Category with all necessary properties set</param>
        /// <returns>A newly created Category</returns>
        /// <response code="201">Return Id of the newly created Category</response>
        /// <response code="400">Unable to successfully process the Category</response>   
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Consumes("application/json")]
        [HttpPost]
        public async Task<ActionResult<int>> AddCategoryAsync([FromBody] CategoryDto category)
        {
            try
            {
                int id = await _categoriesService.CreateAsync(category);
                // CreatedAtAction supplies response with the Location header, which will contain
                // route to get the Category
                // For example
                // Location → .../api/Categories/4
                return CreatedAtAction("GetCategoryAsync", new { id }, id);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Update values of the Category
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT api/categories
        ///     {
        ///     	"id": 2,
        ///     	"name": "NewNameForCategory2"
        ///     }
        /// 
        /// </remarks>
        /// <param name="category">Category with the specified Id and values, that should be changed</param>
        /// <response code="200">Category was updated successfully</response>
        /// <response code="404">Unable to found the Category</response>  
        /// <response code="500">Internal Server Error occured while processing the request</response>        
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [HttpPut]
        public async Task<ActionResult<bool>> EditCategoryAsync([FromBody] CategoryDto category)
        {
            try
            {
                bool success = await _categoriesService.UpdateAsync(category);

                if (success)
                    return Ok(success);
                else
                    return NotFound(string.Format(Resources.FailureResponse, Resources.Update, Resources.Category, Resources.EntityNotFound));
            }
            catch (Exception e)
            {
                return StatusCode(500, string.Format(Resources.FailureResponse, Resources.Update, Resources.Category, e.Message));
            }
        }

        /// <summary>
        /// Delete Category with the specified Id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DELETE api/categories/5
        /// 
        /// </remarks>
        /// <param name="id">Id for the Category that should be deleted</param>
        /// <response code="200">Category was deleted successfully</response>
        /// <response code="404">Unable to found the Category</response>  
        /// <response code="500">Internal Server Error occured while processing the request</response>      
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategoryAsync(int id)
        {
            try
            {
                bool success = await _categoriesService.DeleteAsync(id);

                if (success)
                    return Ok(success);
                else
                    return NotFound(string.Format(Resources.FailureResponse, Resources.Delete, Resources.Category, Resources.EntityNotFound));
            }
            catch (Exception e)
            {
                return StatusCode(500, string.Format(Resources.FailureResponse, Resources.Delete, Resources.Category, e.Message));
            }
        }
    }
}
