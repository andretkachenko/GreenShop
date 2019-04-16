using GreenShop.Catalog.Domain.Categories;
using GreenShop.Catalog.Infrastructure.Products.Interfaces;

namespace GreenShop.Catalog.Infrastructure
{
    public interface IDomainScope : IUnitOfWork
    {
        ISqlProductRepository SqlProductRepository { get; }
        IMongoProductRepository MongoProductRepository { get; }
        IRepository<Category> CategoryRepository { get; }
        ICommentRepository Comments { get; }
    }
}
