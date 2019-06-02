using GreenShop.Catalog.Api.Domain.Products;
using GreenShop.Catalog.Api.Service.Products;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Api.Infrastructure.Products.Interfaces
{
    public interface ISqlProductRepository : IRepository<Product, ProductDto>
    {
        Task<string> GetMongoIdAsync(int id);
    }
}
