using GreenShop.Catalog.Api.Domain;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Api.Infrastructure
{
    public interface IRepository<TEntity, TDto> 
        where TEntity : IEntity
    {
        IDbTransaction Transaction { get; }

        void SetSqlTransaction(IDbTransaction transaction);

        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<int> CreateAsync(TEntity entity);
        Task<bool> UpdateAsync(TDto entity);
    }
}
