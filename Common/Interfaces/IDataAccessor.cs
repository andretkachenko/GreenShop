using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IDataAccessor<T>
    {
        Task<IEnumerable<T>> GetAll();
    }
}
