using Common.Models.Categories;
using Common.Validatiors;
using FluentValidation;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Bff.Shopping.Services.Catalog.Interfaces;

namespace Web.Bff.Shopping.Services.Catalog
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IConsumer<Category> _categoryConsumer;

        public CategoriesService(IConsumer<Category> categoryConsumer)
        {
            _categoryConsumer = categoryConsumer;
        }

        /// <summary>
        /// Asynchronously gets all Categories
        /// </summary>
        /// <returns>Task with list of all Categories</returns>
        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            IEnumerable<Category> categories = await _categoryConsumer.GetAllAsync();

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

            Category category = await _categoryConsumer.GetAsync(id);

            return category;
        }

        /// <summary>
        /// Asynchronously adds Category
        /// </summary>
        /// <param name="category">Category to add</param>
        /// <returns>Task with Category id</returns>
        public async Task<int> AddCategory(Category category)
        {
            EntityNameValidator validator = new EntityNameValidator();
            validator.ValidateAndThrow(category.Name);

            int id = await _categoryConsumer.AddAsync(category);

            return id;
        }

        /// <summary>
        /// Asynchronously edits specified Category
        /// </summary>
        /// <param name="category">Category, that contains id of entity that should be changed, and all changed values</param>
        /// <returns>Task with success flag</returns>
        public async Task<bool> EditCategory(Category category)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(category.Id);

            bool success = await _categoryConsumer.EditAsync(category);

            return success;
        }

        /// <summary>
        /// Asynchronously removed Category with specified id
        /// </summary>
        /// <param name="id">Id of the Category to delete</param>
        /// <returns>Task with success flag</returns>
        public async Task<bool> DeleteCategory(int id)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(id);

            bool success = await _categoryConsumer.DeleteAsync(id);

            return success;
        }
    }
}
