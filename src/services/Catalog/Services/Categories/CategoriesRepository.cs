using FluentValidation;
using GreenShop.Catalog.DataAccessors.Interfaces;
using GreenShop.Catalog.Models.Categories;
using GreenShop.Catalog.Services.Categories.Interfaces;
using GreenShop.Catalog.Validators;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Services.Categories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly ISqlDataAccessor<Category> Categories;

        public CategoriesRepository(ISqlDataAccessor<Category> dataAccessor)
        {
            Categories = dataAccessor;
        }

        /// <summary>
        /// Asynchronously gets all Categories
        /// </summary>
        /// <returns>Task with list of all Categories</returns>
        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            IEnumerable<Category> categories = await Categories.GetAll();

            return categories;
        }

        /// <summary>
        /// Asynchronously gets Category with the specific id
        /// </summary>
        /// <param name="id">Id of the Category to get</param>
        /// <returns>Task with specified Category</returns>
        public async Task<Category> GetCategory(int id)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(id);

            Category category = await Categories.Get(id);

            return category;
        }

        /// <summary>
        /// Asynchronously adds Category
        /// </summary>
        /// <param name="category">Category to add</param>
        /// <returns>Category id</returns>
        public async Task<int> AddCategory(Category category)
        {
            EntityNameValidator validator = new EntityNameValidator();
            validator.ValidateAndThrow(category.Name);

            int id = await Categories.Add(category);

            return id;
        }

        /// <summary>
        /// Asynchronously edits specified Category
        /// </summary>
        /// <param name="category">Category, that contains id of entity that should be changed, and all changed values</param>
        /// <returns>Number of rows affected</returns>
        public async Task<bool> EditCategory(Category category)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(category.Id);

            int rowsAffected = await Categories.Edit(category);

            bool success = rowsAffected == 1;

            return success;
        }

        /// <summary>
        /// Asynchronously removed Category with specified id
        /// </summary>
        /// <param name="id">Id of the Category to delete</param>
        /// <returns>Number of rows affected</returns>
        public async Task<bool> DeleteCategory(int id)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(id);

            int rowsAffected = await Categories.Delete(id);

            bool success = rowsAffected == 1;

            return success;
        }
    }
}
