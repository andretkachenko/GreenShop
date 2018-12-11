using Catalog.Utils;
using Common.Interfaces;
using Common.Models.Categories;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.DataAccessor
{
    public class Categories : IDataAccessor<Category>
    {
        public async Task<IEnumerable<Category>> GetAll()
        {
            using (var context = SqlContext.Context)
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

        public async Task<Category> Get(int id)
        {
            using (var context = SqlContext.Context)
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

        public async Task<int> Add(Category category)
        {
            using (var context = SqlContext.Context)
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

        public async Task<int> Delete(int id)
        {
            using (var context = SqlContext.Context)
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

        public async Task<int> Edit(Category category)
        {
            using (var context = SqlContext.Context)
            {
                var query = @"
                    UPDATE [Categories]
                    SET
                ";

                if(!string.IsNullOrWhiteSpace(category.Name))
                {
                    query += " [Name] = @name";
                }
                if(category.ParentCategoryId != 0)
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
