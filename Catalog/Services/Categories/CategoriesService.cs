using Common.Models.Categories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Services.Categories
{
    public static class CategoriesService
    {
        public static async Task<IEnumerable<Category>> GetAllCategories()
        {
            var categories = await DataAccess.Categories.GetAllCategories();

            return categories;
        }

        public static async Task<Category> GetCategory(int id)
        {
            var category = await DataAccess.Categories.GetCategory(id);

            return category;
        }

        public static async Task<bool> AddCategory(Category category)
        {
            var rowsAffected = await DataAccess.Categories.AddCategory(category);

            var success = rowsAffected == 1;

            return success;
        }

        public static async Task<bool> DeleteCategory(int id)
        {
            var rowsAffected = await DataAccess.Categories.DeleteCategory(id);

            var success = rowsAffected == 1;

            return success;
        }

        public static async Task<bool> EditCategory(Category category)
        {
            var rowsAffected = await DataAccess.Categories.EditCategory(category);

            var success = rowsAffected == 1;

            return success;
        }
    }
}
