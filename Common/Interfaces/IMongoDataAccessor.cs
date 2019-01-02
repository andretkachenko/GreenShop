using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IMongoDataAccessor<T> : IDataAccessor<T>
    {
        Task<T> Get(string id);
        Task Delete(string id);
        Task Add(T entity);
        Task Edit(T entity);
    }
}
