using Common.Models.Categories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Services.Categories.Interfaces
{
    public interface ICategoriesRepository
    {
        Task<IEnumerable<Category>> GetAllCategories();

        Task<Category> GetCategory(int id);

        Task<int> AddCategory(Category category);

        Task<bool> DeleteCategory(int id);

        Task<bool> EditCategory(Category category);
    }
}
