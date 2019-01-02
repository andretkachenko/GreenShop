using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface ISqlDataAccessor<T> : IDataAccessor<T>
    {
        Task<T> Get(int id);
        Task<int> Delete(int id);
        Task<int> Add(T entity);
        Task<int> Edit(T entity);
    }
}
