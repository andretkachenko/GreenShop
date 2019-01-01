using Catalog.Services.Products.Interfaces;
using Common.Interfaces;
using Common.Models.Products;
using Common.Validatiors;
using FluentValidation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Services.Products
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly IDataAccessor<Product> Products;

        public ProductsRepository(IDataAccessor<Product> dataAccessor)
        {
            Products = dataAccessor;
        }

        /// <summary>
        /// Asynchronously gets all Products
        /// </summary>
        /// <returns>Task with list of all Products</returns>
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            var categories = await Products.GetAll();

            return categories;
        }

        /// <summary>
        /// Asynchronously gets Product with the specific id
        /// </summary>
        /// <param name="id">Id of the Product to get</param>
        /// <returns>Task with specified Product</returns>
        public async Task<Product> GetProduct(int id)
        {
            var validator = new IdValidator();
            validator.ValidateAndThrow(id);

            var product = await Products.Get(id);

            return product;
        }

        /// <summary>
        /// Asynchronously adds Product
        /// </summary>
        /// <param name="product">Product to add</param>
        /// <returns>Product id</returns>
        public async Task<int> AddProduct(Product product)
        {
            var validator = new EntityNameValidator();
            validator.ValidateAndThrow(product.Name);

            var id = await Products.Add(product);

            return id;
        }

        /// <summary>
        /// Asynchronously edits specified Product
        /// </summary>
        /// <param name="product">Product, that contains id of entity that should be changed, and all changed values</param>
        /// <returns>Number of rows affected</returns>
        public async Task<bool> EditProduct(Product product)
        {
            var validator = new IdValidator();
            validator.ValidateAndThrow(product.Id);

            var rowsAffected = await Products.Edit(product);

            var success = rowsAffected == 1;

            return success;
        }

        /// <summary>
        /// Asynchronously removed Product with specified id
        /// </summary>
        /// <param name="id">Id of the Product to delete</param>
        /// <returns>Number of rows affected</returns>
        public async Task<bool> DeleteProduct(int id)
        {
            var validator = new IdValidator();
            validator.ValidateAndThrow(id);

            var rowsAffected = await Products.Delete(id);

            var success = rowsAffected == 1;

            return success;
        }
    }
}
