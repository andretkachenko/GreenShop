using Catalog.Utils;
using Common.Configuration.SQL;
using Common.Interfaces;
using Common.Models.Comments;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.DataAccessors
{
    /// <summary>
    /// Comments Dapper Accessor
    /// </summary>
    public class Comments : ISqlChildDataAccessor<Comment>
    {
        public readonly ISqlContext _sql;

        public Comments(ISqlContext sqlContext)
        {
            _sql = sqlContext;
        }

        public async Task<int> Add(Comment comment)
        {
            using (var context = _sql.Context)
            {
                var id = await context.InsertAsync(comment);

                return id;
            }
        }

        public async Task<int> Delete(int id)
        {
            using (var context = _sql.Context)
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
            using (var context = _sql.Context)
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
                if (comment.ProductId > 0)
                {
                    query += "[ProductId] = @productId";
                }

                query += " WHERE [Id] = @id";
                int affectedRows = await context.ExecuteAsync(query, new
                {
                    id = comment.Id,
                    author = comment.Author,
                    message = comment.Message,
                    productId = comment.ProductId
                });
                return affectedRows;
            }
        }

        public async Task<Comment> Get(int id)
        {
            using (var context = _sql.Context)
            {
                var comment = await context.GetAsync<Comment>(id);

                return comment;
            }
        }

        public async Task<IEnumerable<Comment>> GetAll()
        {
            using (var context = _sql.Context)
            {
                var comments = await context.GetAllAsync<Comment>();

                return comments;
            }
        }

        public async Task<IEnumerable<Comment>> GetAllParentRelated(int productId)
        {
            using (var context = _sql.Context)
            {
                var comments = await context.QueryAsync<Comment>(@"
                    SELECT [Id]
                        ,[Author]
                        ,[Message]
                        ,[ProductId]
                    FROM [Contents]
                    WHERE [ProductId] = @productId
                ", new
                {
                    productId
                });

                return comments;
            }
        }
    }
}