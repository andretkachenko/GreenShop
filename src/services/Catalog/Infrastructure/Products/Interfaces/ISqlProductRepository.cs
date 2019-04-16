using GreenShop.Catalog.Domain.Products;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Infrastructure.Products.Interfaces
{
    public interface ISqlProductRepository : IRepository<Product>
    {
        Task<string> GetMongoIdAsync(string id);
    }
}
