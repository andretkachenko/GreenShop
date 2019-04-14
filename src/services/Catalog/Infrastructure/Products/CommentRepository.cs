using Dapper;
using Dapper.Contrib.Extensions;
using GreenShop.Catalog.Config.Interfaces;
using GreenShop.Catalog.Domain.Products;
using GreenShop.Catalog.Infrastructure.Products.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Infrastructure.Products
{
    public class CommentRepository : ICommentRepository
    {
        public readonly ISqlContext _sql;

        public IDbTransaction Transaction { get; private set; }

        public CommentRepository(ISqlContext sqlContext)
        {
            _sql = sqlContext;

        }

        public void SetSqlTransaction(IDbTransaction transaction)
        {
            Transaction = transaction;
        }

        /// <summary>
        /// Asynchronously Add new Comment
        /// </summary>
        /// <param name="comment">Comment to insert to the database</param>
        /// <returns>Task with Id of the Comment</returns>
        public async Task<bool> CreateAsync(Comment comment)
        {
            using (SqlConnection context = _sql.Connection)
            {
                await context.InsertAsync(comment, transaction: Transaction);
                return true;
            }
        }

        public async Task<bool> CreateAsync(IEnumerable<Comment> comments)
        {
            List<Task<bool>> createTasks = new List<Task<bool>>();

            foreach (Comment comment in comments)
            {
                createTasks.Add(CreateAsync(comment));
            }
            await Task.WhenAll(createTasks);
            return createTasks.TrueForAll(x => x.Result);
        }

        /// <summary>
        /// Asynchronously Delete Comment
        /// </summary>
        /// <param name="id">Id of the Comment to delete from database</param>
        /// <returns>Task with number of deleted rows</returns>
        public async Task<bool> DeleteAsync(string id)
        {
            using (SqlConnection context = _sql.Connection)
            {
                string query = @"
                        DELETE
                        FROM [Comments]
                        WHERE [Id] = @id";
                int affectedRows = await context.ExecuteAsync(query, new
                {
                    id
                },
                transaction: Transaction);
                return affectedRows == 1;
            }
        }

        public async Task<bool> UpdateAsync(Guid id, string message)
        {
            using (SqlConnection context = _sql.Connection)
            {
                string query = @"
                        UPDATE [Comments]
                        SET [Message] = @message
                        WHERE [Id] = @id";
                int affectedRows = await context.ExecuteAsync(query, new
                {
                    id,
                    message,
                },
                transaction: Transaction);
                return affectedRows == 1;
            }
        }

        /// <summary>
        /// Asynchronously Edit comment's message
        /// <para>This method calls Edit(int, string) using Id and Message from the Comment</para>
        /// </summary>
        /// <param name="comment">Comment to edit</param>
        /// <returns>Task with the number of proceeded rows</returns>
        public async Task<bool> UpdateAsync(Comment comment) => await UpdateAsync(comment.Id, comment.Message);

        /// <summary>
        /// Asynchronously Get Comment by ID
        /// </summary>
        /// <param name="id">Id of the Comment to get</param>
        /// <returns>Task with Comment</returns>
        public async Task<Comment> GetAsync(string id)
        {
            using (SqlConnection context = _sql.Connection)
            {
                Comment comment = await context.GetAsync<Comment>(id);

                return comment;
            }
        }

        /// <summary>
        /// For this time Not emplemented
        /// </summary>
        /// <returns>NotImplementedException</returns>     
        public Task<IEnumerable<Comment>> GetAllAsync() => throw new NotImplementedException();

        public async Task<IEnumerable<Comment>> GetAllParentRelatedAsync(Guid productId)
        {
            using (SqlConnection context = _sql.Connection)
            {
                IEnumerable<Comment> comments = await context.QueryAsync<Comment>(@"
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

        public async Task<Dictionary<Guid, IEnumerable<Comment>>> GetAllParentRelatedAsync(IEnumerable<Guid> productIds)
        {
            List<Task<IEnumerable<Comment>>> taskList = new List<Task<IEnumerable<Comment>>>();
            Dictionary <Guid, IEnumerable<Comment>> commentsDict = new Dictionary<Guid, IEnumerable<Comment>>();

            foreach (Guid productId in productIds)
            {
                taskList.Add(GetAllParentRelatedAsync(productId));
            }
            await Task.WhenAll(taskList);
            taskList.ForEach(x => commentsDict.Add(x.Result.FirstOrDefault().ProductId, x.Result));

            return commentsDict;
        }

        public async Task<bool> DeleteAllParentRelatedAsync(Guid productId)
        {
            using (SqlConnection context = _sql.Connection)
            {
                string query = @"
                        DELETE
                        FROM [Comments]
                        WHERE [ProductId] = @productId";
                int affectedRows = await context.ExecuteAsync(query, new
                {
                    productId
                },
                transaction: Transaction);
                return affectedRows != 0;
            }
        }
    }
}