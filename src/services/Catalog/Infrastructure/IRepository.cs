using GreenShop.Catalog.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Infrastructure
{
    public interface IRepository<TAggregate> where TAggregate : IAggregate
    {
        IDbTransaction Transaction { get; }

        void SetTransaction(IDbTransaction transaction);

        Task<IEnumerable<TAggregate>> GetAll();
        Task<TAggregate> Get(string id);
        Task<bool> Delete(string id);
        Task<bool> Add(TAggregate entity);
        Task<bool> Edit(TAggregate entity);
    }
}
