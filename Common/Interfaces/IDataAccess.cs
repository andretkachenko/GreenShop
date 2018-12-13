using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IDataAccessor<T> : IDisposable
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(int id);
        Task<int> Add(T entity);
        Task<int> Delete(int id);
        Task<int> Edit(T entity);
    }
}
