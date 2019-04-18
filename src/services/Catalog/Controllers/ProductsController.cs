using GreenShop.Catalog.Service.Products;
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
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productsService;

        public ProductsController(IProductService productsService)
        {
            _productsService = productsService;
        }

        // GET api/products
        [HttpGet]
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            IEnumerable<ProductDto> products = await _productsService.GetAllAsync();

            return products;
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
        public async Task<int> AddProductAsync([FromBody] ProductDto product)
        {
            int id = await _productsService.CreateAsync(product);

            return id;
        }

        // PUT api/products/5
        [HttpPut]
        public async Task<bool> EditProductAsync([FromBody] ProductDto product)
        {
            bool success = await _productsService.UpdateAsync(product);

            return success;
        }

        // DELETE api/products/5
        [HttpDelete("{id}")]
        public async Task<bool> DeleteProductAsync(int id)
        {
            bool success = await _productsService.DeleteAsync(id);

            return success;
        }
    }
}
