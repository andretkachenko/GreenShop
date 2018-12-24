using Catalog.Services.Categories.Interfaces;
using Common.Interfaces;
using Common.Models.Categories;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using Common.Validatiors;

namespace Catalog.Services.Categories
{
    public class CategoriesService : ICategoriesService
    {
        public readonly IDataAccessor<Category> Categories;

        public CategoriesService(IDataAccessor<Category> dataAccessor)
        {
            Categories = dataAccessor;
        }

        /// <summary>
        /// Asynchronously gets all Categories
        /// </summary>
        /// <returns>Task with list of all Categories</returns>
        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            var categories = await Categories.GetAll();

            return categories;
        }

        /// <summary>
        /// Asynchronously gets Category with the specific id
        /// </summary>
        /// <param name="id">Id of the Category to get</param>
        /// <returns>Task with specified Category</returns>
        public async Task<Category> GetCategory(int id)
        {
            var validator = new IdValidator();
            validator.ValidateAndThrow(id);

            var category = await Categories.Get(id);

            return category;
        }

        /// <summary>
        /// Asynchronously adds Category
        /// </summary>
        /// <param name="category">Category to add</param>
        /// <returns>Category id</returns>
        public async Task<int> AddCategory(Category category)
        {
            var validator = new EntityNameValidator();
            validator.ValidateAndThrow(category.Name);

            var id = await Categories.Add(category);

            return id;
        }

        /// <summary>
        /// Asynchronously edits specified Category
        /// </summary>
        /// <param name="category">Category, that contains id of entity that should be changed, and all changed values</param>
        /// <returns>Number of rows affected</returns>
        public async Task<bool> EditCategory(Category category)
        {
            var validator = new IdValidator();
            validator.ValidateAndThrow(category.Id);

            var rowsAffected = await Categories.Edit(category);

            var success = rowsAffected == 1;

            return success;
        }

        /// <summary>
        /// Asynchronously removed Category with specified id
        /// </summary>
        /// <param name="id">Id of the Category to delete</param>
        /// <returns>Number of rows affected</returns>
        public async Task<bool> DeleteCategory(int id)
        {
            var validator = new IdValidator();
            validator.ValidateAndThrow(id);

            var rowsAffected = await Categories.Delete(id);

            var success = rowsAffected == 1;

            return success;
        }
    }
}
