using GreenShop.Catalog.Domain.Categories;
using GreenShop.Catalog.Infrastructure.Products.Interfaces;
using GreenShop.Catalog.Service.Categories;

namespace GreenShop.Catalog.Infrastructure
{
    public interface IDomainScope : IUnitOfWork
    {
        ISqlProductRepository SqlProductRepository { get; }
        IMongoProductRepository MongoProductRepository { get; }
        IRepository<Category, CategoryDto> CategoryRepository { get; }
        ICommentRepository Comments { get; }
    }
}
