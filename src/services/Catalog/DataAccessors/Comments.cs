using Common.Configuration.SQL;
using Common.Interfaces;
using Common.Models.Comments;
using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenShop.Catalog.DataAccessors
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
        /// <param name="comment">Comment to insert to the database</param>
        /// <returns>Task with Id of the Comment</returns>
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
        /// <param name="id">Id of the Comment to delete from database</param>
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
        /// <param name="id">Id of the Comment to edit</param>
        /// <param name="message">Updated message for the Comment</param>
        /// <returns>Task with number of proceeded rows</returns>
        public async Task<int> Edit(int id, string message)
        {
            using (var context = _sql.Context)
            {
                var query = @"
                        UPDATE [Comments]
                        SET [Message] = @message
                        WHERE [Id] = @id";
                int affectedRows = await context.ExecuteAsync(query, new
                {
                    id,
                    message,
                });
                return affectedRows;
            }
        }

        /// <summary>
        /// Asynchronously Edit comment's message
        /// <para>This method calls Edit(int, string) using Id and Message from the Comment</para>
        /// </summary>
        /// <param name="comment">Comment to edit</param>
        /// <returns>Task with the number of proceeded rows</returns>
        public async Task<int> Edit(Comment comment) => await Edit(comment.Id, comment.Message);

        /// <summary>
        /// Asynchronously Get Comment by ID
        /// </summary>
        /// <param name="id">Id of the Comment to get</param>
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
        public Task<IEnumerable<Comment>> GetAll() => throw new NotImplementedException();

        /// <summary>
        /// Asynchronously Get all Comments by product ID
        /// </summary>
        /// <param name="productId">Id of the product to get its comments</param>
        /// <returns>Task with list of comments</returns>
        public async Task<IEnumerable<Comment>> GetAllParentRelated(int productId)
        {
            using (var context = _sql.Context)
            {
                var comments = await context.QueryAsync<Comment>(@"
                    SELECT [Id]
                        ,[AuthorId]
                        ,[Message]
                        ,[ProductId]
                    FROM [Comments]
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