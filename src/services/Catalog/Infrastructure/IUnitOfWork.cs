using System;

namespace GreenShop.Catalog.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        void Rollback();
    }
}
