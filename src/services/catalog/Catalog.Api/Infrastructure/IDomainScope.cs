using GreenShop.Catalog.Api.Domain.Categories;
using GreenShop.Catalog.Api.Infrastructure.Products.Interfaces;
using GreenShop.Catalog.Api.Service.Categories;

namespace GreenShop.Catalog.Api.Infrastructure
{
    public interface IDomainScope : IUnitOfWork
    {
        ISqlProductRepository SqlProductRepository { get; }
        IMongoProductRepository MongoProductRepository { get; }
        IRepository<Category, CategoryDto> CategoryRepository { get; }
        ICommentRepository Comments { get; }
    }
}
