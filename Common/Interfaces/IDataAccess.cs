using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IDataAccessor<T>
    {
        //Task<IEnumerable<T>> GetAll();
        //Task<T> Get(int id);
        //Task<int> Add(T entity);
        //Task<int> Delete(int id);
        //Task<int> Edit(T entity);
    }
    public interface IChildtDataAccessor<T> : IDataAccessor<T>
    {
        Task<IEnumerable<T>> GetAll(int parentId);
        Task<T> Get(int parentId, int id);
        Task<int> Add(int parentId, T entity);
        Task<int> Delete(int parentId, int id);
        Task<int> Edit(int parentId, T entity);
    }
    public interface IParentDataAccessor<T> : IDataAccessor<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(int id);
        Task<int> Add(T entity);
        Task<int> Delete(int id);
        Task<int> Edit(T entity);
    }
}
