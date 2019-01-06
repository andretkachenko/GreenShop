using Common.Models.Entity.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGateway.Services.Catalog.Interfaces
{
    public interface IConsumer<T> where T: IIdentifiable
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(int id);
        Task<int> Add(T entity);
        Task<bool> Delete(int id);
        Task<bool> Edit(T product);
    }
}
