﻿using Catalog.Services.Categories.Interfaces;
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
            var validator = new CategoryIdValidator();
            validator.ValidateAndThrow(id);

            var category = await Categories.Get(id);

            return category;
        }

        /// <summary>
        /// Asynchronously adds Category
        /// </summary>
        /// <param name="category">Category to add</param>
        /// <returns>Number of rows affected</returns>
        public async Task<bool> AddCategory(Category category)
        {
            var validator = new CategoryValidator();
            validator.ValidateAndThrow(category);

            var rowsAffected = await Categories.Add(category);

            var success = rowsAffected == 1;

            return success;
        }

        /// <summary>
        /// Asynchronously edits specified Category
        /// </summary>
        /// <param name="category">Category, that contains id of entity that should be changed, and all changed values</param>
        /// <returns>Number of rows affected</returns>
        public async Task<bool> EditCategory(Category category)
        {
            var validator = new CategoryValidator();
            validator.ValidateAndThrow(category);

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
            var validator = new CategoryIdValidator();
            validator.ValidateAndThrow(id);

            var rowsAffected = await Categories.Delete(id);

            var success = rowsAffected == 1;

            return success;
        }
    }
}
