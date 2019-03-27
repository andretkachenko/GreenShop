using Common.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Services.Products.Interfaces
{
    public interface IProductsRepository
    {
        Task<IEnumerable<Product>> GetAllProducts();

        Task<Product> GetProduct(int id);

        Task<int> AddProduct(Product product);

        Task<bool> DeleteProduct(int id);

        Task<bool> EditProduct(Product product);
    }
}
