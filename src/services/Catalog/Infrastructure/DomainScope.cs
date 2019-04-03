using GreenShop.Catalog.Config.Interfaces;
using GreenShop.Catalog.Models.Categories;
using GreenShop.Catalog.Models.Products;
using MongoDB.Driver;
using System;
using System.Data;

namespace GreenShop.Catalog.Infrastructure
{
    public class DomainScope : IUnitOfWork
    {
        private ISqlContext SqlContext;
        private readonly IMongoContext MongoContext;
        private IDbTransaction _sqlTransaction;
        public IRepository<Product> ProductsRepository {get; private set; }
        public IRepository<Category> CategoriesRepository { get; private set; }
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
            _sqlTransaction = SqlContext.Connection.BeginTransaction();

            IClientSessionHandle mongoSession = MongoContext.Client.StartSession();
            mongoSession.StartTransaction();

            ProductsRepository.SetTransaction(_sqlTransaction);
            CategoriesRepository.SetTransaction(_sqlTransaction);
        }

        public void Commit()
        {
            _sqlTransaction.Commit();
        }

        public void Rollback()
        {
            _sqlTransaction.Rollback();
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
                    if (_sqlTransaction != null)
                    {
                        _sqlTransaction.Dispose();
                        _sqlTransaction = null;
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
