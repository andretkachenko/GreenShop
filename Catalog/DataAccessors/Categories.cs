using Catalog.Utils;
using Common.Interfaces;
using Common.Models.Categories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Catalog.DataAccessor
{
    public class Categories : IDataAccessor<Category>
    {
        private SqlConnection context = SqlContext.Context;
        private bool disposed = false;

        /// <summary>
        /// Asynchronously gets all Categories
        /// </summary>
        /// <returns>Task with list of all Categories</returns>
        public async Task<IEnumerable<Category>> GetAll()
        {
            using (context)
            {
                var categories = await context.QueryAsync<Category>(@"
                    SELECT [Id]
                          ,[Name]
                          ,[ParentCatergoryId]
                    FROM [Categories]
                ");

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
            using (context)
            {
                var category = await context.QueryFirstOrDefaultAsync<Category>(@"
                    SELECT [Id]
                          ,[Name]
                          ,[ParentCatergoryId]
                    FROM [Categories]
                    WHERE [Id] = @id
                ", new
                {
                    id
                });

                return category;
            }
        }

        /// <summary>
        /// Asynchronously adds Category
        /// </summary>
        /// <param name="category">Category to add</param>
        /// <returns>Number of rows affected</returns>
        public async Task<int> Add(Category category)
        {
            using (context)
            {
                var query = string.Empty;

                if (category.ParentCategoryId != 0)
                {
                    query = @"
                    INSERT INTO [Categories]
                        ([Name]
                        ,[ParentCatergoryId])
                    VALUES
                        (@name
                        ,@parentId)";
                }
                else
                {
                    query = @"
                    INSERT INTO [Categories]
                        ([Name])
                    VALUES
                        (@name)";
                }

                var affectedRows = await context.ExecuteAsync(query, new
                {
                    name = category.Name,
                    parentId = category.ParentCategoryId
                });

                return affectedRows;
            }
        }

        /// <summary>
        /// Asynchronously removed Category with specified id
        /// </summary>
        /// <param name="id">Id of the Category to delete</param>
        /// <returns>Number of rows affected</returns>
        public async Task<int> Delete(int id)
        {
            using (context)
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
            using (context)
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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
