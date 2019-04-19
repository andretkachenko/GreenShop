using Dapper;
using Dapper.Contrib.Extensions;
using GreenShop.Catalog.Config.Interfaces;
using GreenShop.Catalog.Domain.Categories;
using GreenShop.Catalog.Infrastructure;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace GreenShop.Catalog.DataAccessor
{
    public class CategoryRepository : IRepository<Category>
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
                    DELETE
                    FROM [Categories]
                    WHERE [Id] = @id
                ", new
            {
                id
            }, transaction: Transaction);

            return affectedRows == 1;
        }

        /// <summary>
        /// Asynchronously edits specified Category
        /// </summary>
        /// <param name="category">Category, that contains id of entity that should be changed, and all changed values</param>
        /// <returns>Number of rows affected</returns>
        public async Task<bool> UpdateAsync(Category category)
        {
            string query = @"
                    UPDATE [Categories]
                    SET
                ";

            if (!string.IsNullOrWhiteSpace(category.Name))
            {
                query += " [Name] = @name";
            }
            if (category.ParentCategoryId != default(int))
            {
                query += " [ParentCategoryId] = @parentId";
            }

            query += " WHERE [Id] = @id";

            int affectedRows = await _sql.Connection.ExecuteAsync(query, new
            {
                id = category.Id,
                name = category.Name,
                parentId = category.ParentCategoryId
            }, transaction: Transaction);

            return affectedRows == 1;
        }
    }
}
