﻿using Common.Models.Comments;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Bff.Shopping.Services.Catalog.Interfaces;

namespace Web.Bff.Shopping.Services.Catalog
{
    public class CommentService : ICommentService
    {
        private readonly ICommentConsumer _commentConsumer;

        public CommentService(ICommentConsumer commentConsumer)
        {
            _commentConsumer = commentConsumer;
        }

        /// <summary>
        ///Asynchronously Add Comment 
        /// </summary>
        /// <param name="comment">Comment to Add</param>
        /// <returns>Task with Comment Id</returns>
        public async Task<int> AddComment(Comment comment)
        {
            int result = await _commentConsumer.AddAsync(comment);

            return result;
        }

        /// <summary>
        /// Asynchronously Delete Comment 
        /// </summary>
        /// <param name="id">Id of Comment to Delete</param>
        /// <returns>Task with boolean result</returns>
        public async Task<bool> DeleteComment(int id)
        {
            bool result = await _commentConsumer.DeleteAsync(id);

            return result;
        }

        /// <summary>
        /// Asynchronously Edit comment's message
        /// <para>This method calls Edit(int, string) using Id and Message from the Comment</para>
        /// </summary>
        /// <param name="comment">Comment to edit</param>
        /// <returns>True if succeeded</returns>
        public async Task<bool> EditComment(int id, string message)
        {
            var result = await _commentConsumer.EditAsync(id, message);

            return result;
        }

        /// <summary>
        /// Asynchronously Edit comment's message
        /// <para>This method calls EditComment(int, string) using Id and Message from the Comment</para>
        /// </summary>
        /// <param name="comment">Comment to edit</param>
        /// <returns>True if succeeded</returns>
        public Task<bool> EditComment(Comment comment) => EditComment(comment.Id, comment.Message);

        /// <summary>
        /// Asynchronously Get all comments for requested product 
        /// </summary>
        /// <param name="productId">Product Id for product</param>
        /// <returns>Task with list of comments</returns>
        public async Task<IEnumerable<Comment>> GetAllProductComments(int productID)
        {
            IEnumerable<Comment> comments = await _commentConsumer.GetAllProductRelatedCommentsAsync(productID);

            return comments;
        }

        /// <summary>
        /// Asynchronously get Comment by Id
        /// </summary>
        /// <param name="id">Id for requested Comment</param>
        /// <returns>Task with Comment</returns>
        public async Task<Comment> GetComment(int id)
        {
            Comment comment = await _commentConsumer.GetAsync(id);

            return comment;
        }
    }
}
