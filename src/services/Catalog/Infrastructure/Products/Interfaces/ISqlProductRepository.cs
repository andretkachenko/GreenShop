using GreenShop.Catalog.Domain.Products;
using GreenShop.Catalog.Service.Products;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Infrastructure.Products.Interfaces
{
    public interface ISqlProductRepository : IRepository<Product, ProductDto>
    {
        Task<string> GetMongoIdAsync(int id);
    }
}
