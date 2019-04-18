using GreenShop.Catalog.Domain;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Infrastructure
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        IDbTransaction Transaction { get; }

        void SetSqlTransaction(IDbTransaction transaction);

        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<int> CreateAsync(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity);
    }
}
