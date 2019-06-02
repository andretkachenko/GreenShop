using Dapper;
using Dapper.Contrib.Extensions;
using GreenShop.Catalog.Api.Config.Interfaces;
using GreenShop.Catalog.Api.Domain.Categories;
using GreenShop.Catalog.Api.Infrastructure;
using GreenShop.Catalog.Api.Service.Categories;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Api.DataAccessor
{
    public class CategoryRepository : IRepository<Category, CategoryDto>
    {
        public readonly ISqlContext _sql;

        public IDbTransaction Transaction { get; private set; }

        public CategoryRepository(ISqlContext sqlContext)
        {
            _sql = sqlContext;
        }

        public void SetSqlTransaction(IDbTransaction transaction)
        {
            Transaction = transaction;
        }

        /// <summary>
        /// Asynchronously gets all Categories
        /// </summary>
        /// <returns>Task with list of all Categories</returns>
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            IEnumerable<Category> categories = await _sql.Connection.GetAllAsync<Category>();

            return categories;
        }

        /// <summary>
        /// Asynchronously gets Category with the specific id
        /// </summary>
        /// <param name="id">Id of the Category to get</param>
        /// <returns>Task with specified Category</returns>
        public async Task<Category> GetAsync(int id)
        {
            Category category = await _sql.Connection.GetAsync<Category>(id);

            return category;
        }

        /// <summary>
        /// Asynchronously adds Category
        /// </summary>
        /// <param name="category">Category to add</param>
        /// <returns>Category Id</returns>
        public async Task<int> CreateAsync(Category category)
        {
            int id = await _sql.Connection.InsertAsync(category, transaction: Transaction);

            return id;
        }

        /// <summary>
        /// Asynchronously removed Category with specified id
        /// </summary>
        /// <param name="id">Id of the Category to delete</param>
        /// <returns>Number of rows affected</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            int affectedRows = await _sql.Connection.ExecuteAsync(@"
                    UPDATE [Categories]
                    SET [StatusCode] = @status 
                    WHERE [Id] = @id
                ", new
            {
                id,
                status = CategoryStatus.Archived
            }, transaction: Transaction);

            return affectedRows == 1;
        }

        /// <summary>
        /// Asynchronously edits specified Category
        /// </summary>
        /// <param name="categoryDto">Category, that contains id of entity that should be changed, and all changed values</param>
        /// <returns>Number of rows affected</returns>
        public async Task<bool> UpdateAsync(CategoryDto categoryDto)
        {
            string query = @"
                    UPDATE [Categories]
                    SET
                ";

            if(categoryDto.StatusCode != default)
            {
                query += " [StatusCode] = @statusCode";
            }
            if (!string.IsNullOrWhiteSpace(categoryDto.Name))
            {
                query += " [Name] = @name";
            }
            if (categoryDto.ParentCategoryId != default)
            {
                query += " [ParentCategoryId] = @parentId";
            }

            query += " WHERE [Id] = @id";

            int affectedRows = await _sql.Connection.ExecuteAsync(query, new
            {
                id = categoryDto.Id,
                name = categoryDto.Name,
                parentId = categoryDto.ParentCategoryId,
                statusCode = categoryDto.StatusCode
            }, transaction: Transaction);

            return affectedRows == 1;
        }
    }
}
