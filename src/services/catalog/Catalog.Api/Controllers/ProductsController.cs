using GreenShop.Catalog.Api.Service.Products;
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
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productsService;

        public ProductsController(IProductService productsService)
        {
            _productsService = productsService;
        }

        // GET api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProductsAsync()
        {
            try
            {
                IEnumerable<ProductDto> products = await _productsService.GetAllAsync();
                if (products == null) throw new ArgumentNullException();
                return Ok(products);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        // GET api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductAsync(int id)
        {
            try
            {
                ProductDto product = await _productsService.GetAsync(id);
                if (product == null) throw new ArgumentNullException();
                return Ok(product);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
        }

        // POST api/products
        [HttpPost]
        public async Task<ActionResult<int>> AddProductAsync([FromBody] ProductDto product)
        {
            try
            {
                int id = await _productsService.CreateAsync(product);
                return Ok(id);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // PUT api/products/5
        [HttpPut]
        public async Task<ActionResult<bool>> EditProductAsync([FromBody] ProductDto product)
        {
            try
            {
                bool success = await _productsService.UpdateAsync(product);
                return Ok(success);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // DELETE api/products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteProductAsync(int id)
        {
            try
            {
                bool success = await _productsService.DeleteAsync(id);
                return Ok(success);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
