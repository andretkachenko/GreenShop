using GreenShop.Catalog.Domain.Products;

namespace GreenShop.Catalog.Infrastructure.Products.Interfaces
{
    public interface IMongoProductRepository : IRepository<Product>
    {
    }
}
