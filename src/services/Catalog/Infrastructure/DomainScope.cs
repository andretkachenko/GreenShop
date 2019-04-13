using GreenShop.Catalog.Config.Interfaces;
using GreenShop.Catalog.Domain.Categories;
using GreenShop.Catalog.Domain.Products;
using GreenShop.Catalog.Infrastructure.Products.Interfaces;
using MongoDB.Driver;
using System;
using System.Data;

namespace GreenShop.Catalog.Infrastructure
{
    public class DomainScope : IUnitOfWork
    {
        private ISqlContext SqlContext;
        private readonly IMongoContext MongoContext;
        private IDbTransaction SqlTransaction;
        private IClientSessionHandle MongoSession;
        public IRepository<Product> ProductsRepository { get; private set; }
        public IRepository<Category> CategoriesRepository { get; private set; }
        public ICommentRepository Comments { get; private set; }
        private bool _disposed = false;

        public DomainScope(ISqlContext sqlContext,
            IMongoContext mongoContext,
            IRepository<Product> productsRepository,
            IRepository<Category> categoriesRepository,
            ICommentRepository comments)
        {
            SqlContext = sqlContext;
            MongoContext = mongoContext;
            ProductsRepository = productsRepository;
            CategoriesRepository = categoriesRepository;
            Comments = comments;
        }

        public void Begin()
        {
            SqlContext.Connection.Open();
            SqlTransaction = SqlContext.Connection.BeginTransaction();

            MongoSession = MongoContext.Client.StartSession();
            MongoSession.StartTransaction();

            ProductsRepository.SetSqlTransaction(SqlTransaction);
            CategoriesRepository.SetSqlTransaction(SqlTransaction);
        }

        public void Commit()
        {
            SqlTransaction.Commit();
            MongoSession.CommitTransactionAsync();
        }

        public void Rollback()
        {
            SqlTransaction.Rollback();
            MongoSession.AbortTransactionAsync();
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
                    if (SqlTransaction != null)
                    {
                        SqlTransaction.Dispose();
                        SqlTransaction = null;
                    }
                    if (SqlContext != null)
                    {
                        SqlContext.Dispose();
                        SqlContext = null;
                    }
                    if (MongoSession != null)
                    {
                        MongoSession.Dispose();
                        MongoSession = null;
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
