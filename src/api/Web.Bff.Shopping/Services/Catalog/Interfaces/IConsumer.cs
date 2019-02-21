using Common.Models.Entity.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web.Bff.Shopping.Services.Catalog.Interfaces
{
    public interface IConsumer<T> where T: IIdentifiable
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetAsync(int id);
        Task<int> AddAsync(T entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> EditAsync(T product);
    }
}
