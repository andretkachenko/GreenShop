using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Services.Products.Interfaces;
using Common.Models.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;

        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        // GET api/products
        [HttpGet]
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var products = await _productsService.GetAllProducts();

            return products;
        }

        // GET api/products/5
        [HttpGet("{id}")]
        public async Task<Product> GetProductAsync(int id)
        {
            var product = await _productsService.GetProduct(id);

            return product;
        }

        // POST api/products
        [HttpPost]
        public async Task<int> AddProductAsync([FromBody] Product product)
        {
            var success = await _productsService.AddProduct(product);

            return success;
        }

        // PUT api/products/5
        [HttpPut]
        public async Task<bool> EditProductAsync([FromBody] Product product)
        {
            var success = await _productsService.EditProduct(product);

            return success;
        }

        // DELETE api/products/5
        [HttpDelete("{id}")]
        public async Task<bool> DeleteProductAsync(int id)
        {
            var success = await _productsService.DeleteProduct(id);

            return success;
        }
    }
}
