using GreenShop.Catalog.Domain.Categories;
using GreenShop.Catalog.Domain.Products;
using GreenShop.Catalog.Infrastructure.Products.Interfaces;

namespace GreenShop.Catalog.Infrastructure
{
    public interface IDomainScope : IUnitOfWork
    {
        ISqlProductRepository SqlProducts { get; }
        IMongoProductRepository MongoProducts { get; }
        IRepository<Category> CategoriesRepository { get; }
        ICommentRepository Comments { get; }
    }
}
