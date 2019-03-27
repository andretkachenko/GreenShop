using GreenShop.Catalog.Services.Products.Interfaces;
using Common.Models.Products;
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
        private readonly IProductsRepository _productsService;

        public ProductsController(IProductsRepository productsService)
        {
            _productsService = productsService;
        }

        // GET api/products
        [HttpGet]
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            IEnumerable<Product> products = await _productsService.GetAllProducts();

            return products;
        }

        // GET api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductAsync(int id)
        {
            try
            {
                Product product = await _productsService.GetProduct(id);
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
        public async Task<int> AddProductAsync([FromBody] Product product)
        {
            int success = await _productsService.AddProduct(product);

            return success;
        }

        // PUT api/products/5
        [HttpPut]
        public async Task<bool> EditProductAsync([FromBody] Product product)
        {
            bool success = await _productsService.EditProduct(product);

            return success;
        }

        // DELETE api/products/5
        [HttpDelete("{id}")]
        public async Task<bool> DeleteProductAsync(int id)
        {
            bool success = await _productsService.DeleteProduct(id);

            return success;
        }
    }
}
