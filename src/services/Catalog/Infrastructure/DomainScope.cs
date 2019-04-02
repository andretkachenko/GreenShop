using GreenShop.Catalog.Config.Interfaces;
using GreenShop.Catalog.Models.Categories;
using GreenShop.Catalog.Models.Products;
using System;
using System.Data;

namespace GreenShop.Catalog.Infrastructure
{
    public class DomainScope : IUnitOfWork
    {
        private ISqlContext SqlContext;
        private readonly IMongoContext MongoContext;
        private IDbTransaction _transaction;
        private readonly IRepository<Product> ProductsRepository;
        private readonly IRepository<Category> CategoriesRepository;
        private bool _disposed = false;

        public DomainScope(ISqlContext sqlContext,
            IMongoContext mongoContext,
            IRepository<Product> productsRepository,
            IRepository<Category> categoriesRepository)
        {
            SqlContext = sqlContext;
            MongoContext = mongoContext;
            ProductsRepository = productsRepository;
            CategoriesRepository = categoriesRepository;
        }

        public void Begin()
        {
            SqlContext.Connection.Open();
            _transaction = SqlContext.Connection.BeginTransaction();
            ProductsRepository.SetTransaction(_transaction);
            CategoriesRepository.SetTransaction(_transaction);
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    if (SqlContext != null)
                    {
                        SqlContext.Dispose();
                        SqlContext = null;
                    }
                }
                _disposed = true;
            }
        }

        ~DomainScope()
        {
            Dispose(false);
        }
    }
}
