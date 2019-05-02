using GreenShop.Catalog.Api.Domain.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Api.Infrastructure.Products.Interfaces
{
    public interface IMongoProductRepository
    {
        /// <summary>
        /// Asynchronously get all Products
        /// </summary>
        /// <returns>Task with list of all Products</returns>
        Task<IEnumerable<Product>> GetAllAsync();

        /// <summary>
        /// Asynchronously get Product with the specific id
        /// </summary>
        /// <param name="id">Id of the Product to get</param>
        /// <returns>Task with specified Product</returns>
        Task<Product> GetAsync(string id);

        /// <summary>
        /// Asynchronously adds Product
        /// </summary>
        /// <param name="product">Product to add</param>
        /// <returns>Task with specified Category</returns>
        Task<bool> CreateAsync(Product product);

        /// <summary>
        /// Asynchronously remove Product with specified id
        /// </summary>
        /// <param name="id">Id of the Product to delete</param>
        /// <returns>Number of rows affected</returns>
        Task<bool> DeleteAsync(string id);

        /// <summary>
        /// Asynchronously update specified Product
        /// </summary>
        /// <param name="product">Product, that contains id of entity that should be changed, and all changed values</param>
        /// <returns>Number of rows affected</returns>
        Task<bool> UpdateAsync(Product product);
    }
}
