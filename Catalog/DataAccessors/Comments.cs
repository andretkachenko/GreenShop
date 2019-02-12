using Catalog.Utils;
using Common.Configuration.SQL;
using Common.Interfaces;
using Common.Models.Comments;
using Common.Validatiors.Comments;
using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.DataAccessors
{
    public class Comments : ISqlChildDataAccessor<Comment>
    {
        public readonly ISqlContext _sql;
        
        public Comments(ISqlContext sqlContext)
        {
            _sql = sqlContext;

        }

        /// <summary>
        /// Asynchronously Add new Comment
        /// </summary>
        /// <param name="comment"></param>
        /// <returns>Task with ID</returns>
        public async Task<int> Add(Comment comment)
        {
            using (var context = _sql.Context)
            {
                var id = await context.InsertAsync(comment);

                return id;
            }
        }

        /// <summary>
        /// Asynchronously Delete Comment
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Task with number of deleted rows</returns>
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

        /// <summary>
        /// Asynchronously Edit comment's message
        /// </summary>
        /// <param name="id"></param>
        /// <param name="message"></param>
        /// <returns>Task with numer of proceeded rows</returns>
        public async Task<int> Edit(int id, string message)
        {
            using (var context = _sql.Context)
            {
                var query = @"
                        UPDATE [Comments]
                        SET";
                if (!string.IsNullOrWhiteSpace(message))
                {
                    query += "[Message] = @message";
                }

                query += " WHERE [Id] = @id";
                int affectedRows = await context.ExecuteAsync(query, new
                {
                    id = id,
                    message = message,
                });
                return affectedRows;
            }
        }

        /// <summary>
        /// For this time Not emplemented
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>NotImplementedException</returns>
        public async Task<int> Edit(Comment entity) => throw new NotImplementedException();

        /// <summary>
        /// Asynchronously Get Comment by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Task with Comment</returns>
        public async Task<Comment> Get(int id)
        {
            using (var context = _sql.Context)
            {
                var comment = await context.GetAsync<Comment>(id);

                return comment;
            }
        }

        /// <summary>
        /// For this time Not emplemented
        /// </summary>
        /// <returns>NotImplementedException</returns>
        public async Task<IEnumerable<Comment>> GetAll() => throw new NotImplementedException();

        /// <summary>
        /// Asynchronously Get all Comments by product ID
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>Task with list of comments</returns>
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