using AutoMapper;
using FluentValidation;
using GreenShop.Catalog.Domain.Categories;
using GreenShop.Catalog.Infrastructure;
using GreenShop.Catalog.Validators;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Service.Categories
{
    public class CategoryService : ICategoryService
    {
        private IDomainScope Scope;

        public CategoryService(IDomainScope unitOfWork)
        {
            Scope = unitOfWork;
        }

        /// <summary>
        /// Asynchronously gets all Categories
        /// </summary>
        /// <returns>Task with list of all Categories</returns>
        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            using (Scope)
            {
                IEnumerable<Category> categories = await Scope.CategoryRepository.GetAllAsync();

                IEnumerable<CategoryDto> result = Mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDto>>(categories);
                return result;
            }
        }

        /// <summary>
        /// Asynchronously get Category with the specific id
        /// </summary>
        /// <param name="id">Id of the Category to get</param>
        /// <returns>Task with specified Category</returns>
        public async Task<CategoryDto> GetAsync(int id)
        {
            using (Scope)
            {
                Category category = await Scope.CategoryRepository.GetAsync(id);

                CategoryDto result = Mapper.Map<Category, CategoryDto>(category);
                return result;
            }
        }

        /// <summary>
        /// Asynchronously adds Category
        /// </summary>
        /// <param name="categoryDto">Category to add</param>
        /// <returns>Category id</returns>
        public async Task<int> CreateAsync(CategoryDto categoryDto)
        {
            EntityNameValidator nameValidator = new EntityNameValidator();
            nameValidator.ValidateAndThrow(categoryDto.Name);

            Category category = new Category(categoryDto.Name, categoryDto.ParentCategoryId);

            using (Scope)
            {
                try
                {
                    Scope.Begin();
                    int id = await Scope.CategoryRepository.CreateAsync(category);
                    Scope.Commit();

                    return id;
                }
                catch (Exception e)
                {
                    Scope.Rollback();
                    throw e;
                }
            }
        }

        /// <summary>
        /// Asynchronously edits specified Category
        /// </summary>
        /// <param name="categoryDto">Category, that contains id of entity that should be changed, and all changed values</param>
        /// <returns>Number of rows affected</returns>
        public async Task<bool> UpdateAsync(CategoryDto categoryDto)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(categoryDto.Id);

            Category category = new Category(categoryDto.Name, categoryDto.ParentCategoryId);

            using (Scope)
            {
                try
                {
                    Scope.Begin();
                    bool success = await Scope.CategoryRepository.UpdateAsync(category);
                    Scope.Commit();

                    return success;
                }
                catch (Exception e)
                {
                    Scope.Rollback();
                    throw e;
                }
            }
        }

        /// <summary>
        /// Asynchronously removed Category with specified id
        /// </summary>
        /// <param name="id">Id of the Category to delete</param>
        /// <returns>True on success</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(id);

            using (Scope)
            {
                try
                {
                    Scope.Begin();
                    bool success = await Scope.CategoryRepository.DeleteAsync(id);
                    Scope.Commit();

                    return success;
                }
                catch (Exception e)
                {
                    Scope.Rollback();
                    throw e;
                }
            }
        }
    }
}
