using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenShop.Catalog.DataAccessors.Interfaces
{
    public interface ISqlChildDataAccessor<T> : ISqlDataAccessor<T>
    {
        Task<IEnumerable<T>> GetAllParentRelated(int parentId);
        Task<int> Edit(int parentId, string message);
    }
}
