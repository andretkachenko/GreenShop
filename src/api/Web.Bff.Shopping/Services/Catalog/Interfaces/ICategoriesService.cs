using Common.Models.Categories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web.Bff.Shopping.Services.Catalog.Interfaces
{
    public interface ICategoriesService
    {
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category> GetCategory(int id);
        Task<int> AddCategory(Category category);
        Task<bool> EditCategory(Category category);
        Task<bool> DeleteCategory(int id);
    }
}
