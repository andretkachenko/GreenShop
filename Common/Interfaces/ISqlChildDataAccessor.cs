using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface ISqlChildDataAccessor<T> : ISqlDataAccessor<T>
    {
        Task<IEnumerable<T>> GetAllParentRelated(int parentId);
    }
}
