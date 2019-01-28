using Catalog.Utils;
using Common.Interfaces;
using Common.Models.Comments;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.DataAccessors
{
    /// <summary>
    /// Comments Dapper Accessor
    /// </summary>
    public class Comments : IDataAccessor<Comment>
    {
        public async Task<int> Add(Comment comment)
        {
            using (var context = SqlContext.Context)
            {
                var query = @"
                INSERT INTO [Comments]
                    ([Author],
                    [Message])
                VALUES
                    (@author,
                    @message)";


                int affectedRows = await context.ExecuteAsync(query, new
                {
                    author = comment.Author,
                    message = comment.Message
                });

                return affectedRows;
            }
        }

        public async Task<int> Delete(int id)
        {
            using (var context = SqlContext.Context)
            {
                var query = @"
                        DELETE
                        FROM [Comments]
                        WHERE [Id] = @id";
                int affectedRows = await context.ExecuteAsync(query, new
                {
                    id
                });
                return affectedRows;
            }
        }

        public async Task<int> Edit(Comment comment)
        {
            using (var context = SqlContext.Context)
            {
                var query = @"
                        UPDATE [Comments]
                        SET";
                if (!string.IsNullOrWhiteSpace(comment.Author))
                {
                    query += "[Author] = @author";
                }
                if (!string.IsNullOrWhiteSpace(comment.Message))
                {
                    query += "[Message] = @message";
                }

                query += " WHERE [Id] = @id";
                int affectedRows = await context.ExecuteAsync(query, new
                {
                    id = comment.Id,
                    author = comment.Author,
                    message = comment.Message
                });
                return affectedRows;
            }
        }

        public async Task<Comment> Get(int id)
        {
            using (var context = SqlContext.Context)
            {
                var query = @"
                  SELECT [Id]
                        ,[Author]
                        ,[Message]
                  FROM [Contents]
                  WHERE [Id] = @id";
                Comment comment = await context.QueryFirstOrDefaultAsync<Comment>(query, new
                {
                    id
                });
                return comment;
            }
        }

        public async Task<IEnumerable<Comment>> GetAll()
        {
            using (var context = SqlContext.Context)
            {
                var query = @"
                  SELECT [Id]
                        ,[Author]
                        ,[Message]
                  FROM [Contents]";
                IEnumerable<Comment> comments = await context.QueryAsync<Comment>(query);

                return comments;
            }
        }
    }
}