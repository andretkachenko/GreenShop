using Common.Configuration.SQL;
using Common.Interfaces;
using Common.Models.Categories;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenShop.Catalog.DataAccessor
{
    public class Categories : ISqlDataAccessor<Category>
    {
        public readonly ISqlContext _sql;

        public Categories(ISqlContext sqlContext)
        {
            _sql = sqlContext;
        }

        /// <summary>
        /// Asynchronously gets all Categories
        /// </summary>
        /// <returns>Task with list of all Categories</returns>
        public async Task<IEnumerable<Category>> GetAll()
        {
            using (var context = _sql.Context)
            {
                var categories = await context.GetAllAsync<Category>();

                return categories;
            }
        }

        /// <summary>
        /// Asynchronously gets Category with the specific id
        /// </summary>
        /// <param name="id">Id of the Category to get</param>
        /// <returns>Task with specified Category</returns>
        public async Task<Category> Get(int id)
        {
            using (var context = _sql.Context)
            {
                var category = await context.GetAsync<Category>(id);

                return category;
            }
        }

        /// <summary>
        /// Asynchronously adds Category
        /// </summary>
        /// <param name="category">Category to add</param>
        /// <returns>Category Id</returns>
        public async Task<int> Add(Category category)
        {
            using (var context = _sql.Context)
            {
                var id = await context.InsertAsync(category);

                return id;
            }
        }

        /// <summary>
        /// Asynchronously removed Category with specified id
        /// </summary>
        /// <param name="id">Id of the Category to delete</param>
        /// <returns>Number of rows affected</returns>
        public async Task<int> Delete(int id)
        {
            using (var context = _sql.Context)
            {
                var affectedRows = await context.ExecuteAsync(@"
                    DELETE
                    FROM [Categories]
                    WHERE [Id] = @id
                ", new
                {
                    id
                });

                return affectedRows;
            }
        }

        /// <summary>
        /// Asynchronously edits specified Category
        /// </summary>
        /// <param name="category">Category, that contains id of entity that should be changed, and all changed values</param>
        /// <returns>Number of rows affected</returns>
        public async Task<int> Edit(Category category)
        {
            using (var context = _sql.Context)
            {
                var query = @"
                    UPDATE [Categories]
                    SET
                ";

                if (!string.IsNullOrWhiteSpace(category.Name))
                {
                    query += " [Name] = @name";
                }
                if (category.ParentCategoryId != 0)
                {
                    query += " [ParentCategoryId] = @parentId";
                }

                query += " WHERE [Id] = @id";

                var affectedRows = await context.ExecuteAsync(query, new
                {
                    id = category.Id,
                    name = category.Name,
                    parentId = category.ParentCategoryId
                });

                return affectedRows;
            }
        }
    }
}
