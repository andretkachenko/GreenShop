using Common.Models.Products;
using Common.Validatiors;
using FluentValidation;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Bff.Shopping.Services.Catalog.Interfaces;

namespace Web.Bff.Shopping.Services.Catalog
{
    public class ProductsService : IProductsService
    {
        private readonly IConsumer<Product> _productConsumer;

        public ProductsService(IConsumer<Product> productConsumer)
        {
            _productConsumer = productConsumer;
        }

        /// <summary>
        /// Asynchronously get all Products
        /// </summary>
        /// <returns>Task with list of all Products</returns>
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            IEnumerable<Product> products = await _productConsumer.GetAllAsync();

            return products;
        }

        /// <summary>
        /// Asynchronously get Product with the specific id
        /// </summary>
        /// <param name="id">Id of the Product to get</param>
        /// <returns>Task with specified Product</returns>
        public async Task<Product> GetProduct(int id)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(id);

            Product product = await _productConsumer.GetAsync(id);

            return product;
        }

        /// <summary>
        /// Asynchronously add Product
        /// </summary>
        /// <param name="Product">Product to add</param>
        /// <returns>Task with Product id</returns>
        public async Task<int> AddProduct(Product product)
        {
            EntityNameValidator validator = new EntityNameValidator();
            validator.ValidateAndThrow(product.Name);

            int id = await _productConsumer.AddAsync(product);
            return id;
        }

        /// <summary>
        /// Asynchronously edit specified Product
        /// </summary>
        /// <param name="Product">Product, that contains id of entity that should be changed, and all changed values</param>
        /// <returns>Task with success flag</returns>
        public async Task<bool> EditProduct(Product product)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(product.Id);

            bool success = await _productConsumer.EditAsync(product);

            return success;
        }

        /// <summary>
        /// Asynchronously remove Product with specified id
        /// </summary>
        /// <param name="id">Id of the Product to delete</param>
        /// <returns>Task with success flag</returns>
        public async Task<bool> DeleteProduct(int id)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(id);

            bool success = await _productConsumer.DeleteAsync(id);

            return success;
        }
    }
}
