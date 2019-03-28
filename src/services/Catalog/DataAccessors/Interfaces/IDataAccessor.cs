using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenShop.Catalog.DataAccessors.Interfaces
{
    public interface IDataAccessor<T>
    {
        Task<IEnumerable<T>> GetAll();
    }
}
