using Catalog.Services.Categories.Interfaces;
using Common.Interfaces;
using Common.Models.Categories;
using Common.Validatiors.Categories;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;

namespace Catalog.Services.Categories
{
    public class CategoriesService : ICategoriesService
    {
        public readonly IParentDataAccessor<Category> Categories;

        public CategoriesService(IParentDataAccessor<Category> dataAccessor)
        {
            Categories = dataAccessor;
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            var categories = await Categories.GetAll();

            return categories;
        }

        public async Task<Category> GetCategory(int id)
        {
            var validator = new CategoryIdValidator();
            validator.ValidateAndThrow(id);

            var category = await Categories.Get(id);

            return category;
        }

        public async Task<bool> AddCategory(Category category)
        {
            var validator = new CategoryValidator();
            validator.ValidateAndThrow(category);

            var rowsAffected = await Categories.Add(category);

            var success = rowsAffected == 1;

            return success;
        }

        public async Task<bool> EditCategory(Category category)
        {
            var validator = new CategoryValidator();
            validator.ValidateAndThrow(category);

            var rowsAffected = await Categories.Edit(category);

            var success = rowsAffected == 1;

            return success;
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var validator = new CategoryIdValidator();
            validator.ValidateAndThrow(id);

            var rowsAffected = await Categories.Delete(id);

            var success = rowsAffected == 1;

            return success;
        }
    }
}
