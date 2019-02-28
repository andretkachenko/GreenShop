using Common.Models.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web.Bff.Shopping.Services.Catalog.Interfaces
{
    public interface IProductsService
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProduct(int id);
        Task<int> AddProduct(Product product);
        Task<bool> EditProduct(Product product);
        Task<bool> DeleteProduct(int id);
    }
}
