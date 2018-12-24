using Common.Models.Categories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Services.Categories.Interfaces
{
    public interface ICategoriesService
    {
        Task<IEnumerable<Category>> GetAllCategories();

        Task<Category> GetCategory(int id);

        Task<int> AddCategory(Category category);

        Task<bool> DeleteCategory(int id);

        Task<bool> EditCategory(Category category);
    }
}
