using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Service.Categories
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto> GetAsync(string id);
        Task<Guid> CreateAsync(CategoryDto product);
        Task<bool> UpdateAsync(CategoryDto product);
        Task<bool> DeleteAsync(string id);
    }
}
