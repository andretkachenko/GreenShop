using Dapper;
using Dapper.Contrib.Extensions;
using GreenShop.Catalog.Config.Interfaces;
using GreenShop.Catalog.Domain.Products;
using GreenShop.Catalog.Infrastructure.Products.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
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
        public async Task<int> CreateAsync(Comment comment)
        {
            int result = await _sql.Connection.InsertAsync(comment, transaction: Transaction);
            return result;
        }

        public async Task<IEnumerable<int>> CreateAsync(IEnumerable<Comment> comments)
        {
            List<int> ids = new List<int>();
            List<Task<int>> createTasks = new List<Task<int>>();

            foreach (Comment comment in comments)
            {
                createTasks.Add(CreateAsync(comment));
            }
            await Task.WhenAll(createTasks);
            createTasks.ForEach(x => ids.Add(x.Result));
            return ids;
        }

        /// <summary>
        /// Asynchronously Delete Comment
        /// </summary>
        /// <param name="id">Id of the Comment to delete from database</param>
        /// <returns>Task with number of deleted rows</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            string query = @"
                        DELETE
                        FROM [Comments]
                        WHERE [Id] = @id";
            int affectedRows = await _sql.Connection.ExecuteAsync(query, new
            {
                id
            },
            transaction: Transaction);
            return affectedRows == 1;
        }

        public async Task<bool> UpdateAsync(int id, string message)
        {
            string query = @"
                        UPDATE [Comments]
                        SET [Message] = @message
                        WHERE [Id] = @id";
            int affectedRows = await _sql.Connection.ExecuteAsync(query, new
            {
                id,
                message,
            },
            transaction: Transaction);
            return affectedRows == 1;
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
        public async Task<Comment> GetAsync(int id)
        {
            Comment comment = await _sql.Connection.GetAsync<Comment>(id);

            return comment;
        }

        /// <summary>
        /// For this time Not emplemented
        /// </summary>
        /// <returns>NotImplementedException</returns>     
        public Task<IEnumerable<Comment>> GetAllAsync() => throw new NotImplementedException();

        public async Task<IEnumerable<Comment>> GetAllParentRelatedAsync(int productId)
        {
            IEnumerable<Comment> comments = await _sql.Connection.QueryAsync<Comment>(@"
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

        public async Task<Dictionary<int, IEnumerable<Comment>>> GetAllParentRelatedAsync(IEnumerable<int> productIds)
        {
            List<Task<IEnumerable<Comment>>> taskList = new List<Task<IEnumerable<Comment>>>();
            Dictionary<int, IEnumerable<Comment>> commentsDict = new Dictionary<int, IEnumerable<Comment>>();

            foreach (int productId in productIds)
            {
                taskList.Add(GetAllParentRelatedAsync(productId));
            }
            await Task.WhenAll(taskList);
            taskList.ForEach(x => commentsDict.Add(x.Result.FirstOrDefault().ProductId, x.Result));

            return commentsDict;
        }

        public async Task<bool> DeleteAllParentRelatedAsync(int productId)
        {
            string query = @"
                        DELETE
                        FROM [Comments]
                        WHERE [ProductId] = @productId";
            int affectedRows = await _sql.Connection.ExecuteAsync(query, new
            {
                productId
            },
            transaction: Transaction);
            return affectedRows != 0;
        }
    }
}