using Catalog.Services.Comments.Interfaces;
using Common.Interfaces;
using Common.Models.Comments;
using Common.Validatiors;
using Common.Validatiors.Comments;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Services.Comments
{
    public class CommentsRepository : ICommentsRepository
    {
        public readonly ISqlChildDataAccessor<Comment> Comments;

        public CommentsRepository(ISqlChildDataAccessor<Comment> dataAccessor)
        {
            Comments = dataAccessor;
        }

        /// <summary>
        ///Asynchronously adds Comment 
        /// </summary>
        /// <param name="comment">Comment to add</param>
        /// <returns>True if succeeded</returns>
        public async Task<bool> AddComment(Comment comment)
        {
            var validator = new CommentValidator();
            validator.ValidateAndThrow(comment);

            var rowsAffected = await Comments.Add(comment);
            var resul = rowsAffected == 1;
            return resul;
        }

        /// <summary>
        /// Asynchronously Deletes Comment by Id
        /// </summary>
        /// <param name="id">Id of the Comment to delete</param>
        /// <returns>True if succeeded</returns>
        public async Task<bool> DeleteComment(int id)
        {
            var validator = new IdValidator();
            validator.ValidateAndThrow(id);

            var rowsAffected = await Comments.Delete(id);
            var result = rowsAffected == 1;
            return result;
        }

        /// <summary>
        /// Asynchronously Edit comment's message
        /// <para>This method calls Edit(int, string) using Id and Message from the Comment</para>
        /// </summary>
        /// <param name="comment">Comment to edit</param>
        /// <returns>True if succeeded</returns>
        public Task<bool> EditComment(Comment comment) => EditComment(comment.Id, comment.Message);

        /// <summary>
        /// Asynchronously Edit comment's message
        /// </summary>
        /// <param name="id">Id of the Comment to edit</param>
        /// <param name="message">Updated message for the comment</param>
        /// <returns>True if succeeded</returns>
        public async Task<bool> EditComment(int id, string message)
        {
            var idValidator = new IdValidator();
            idValidator.ValidateAndThrow(id);

            var messageValidator = new CommentMessageValidator();
            messageValidator.ValidateAndThrow(message);

            var rowsAffected = await Comments.Edit(id, message);
            var result = rowsAffected == 1;
            return result;
        }

        /// <summary>
        /// Gets all Comments by Product Id
        /// </summary>
        /// <param name="productID">Id of the product to get its comments</param>
        /// <returns>Task with list of all comments</returns>
        public async Task<IEnumerable<Comment>> GetAllProductComments(int productID)
        {
            var idValidator = new IdValidator();
            idValidator.ValidateAndThrow(productID);

            var comments = await Comments.GetAllParentRelated(productID);
            return comments;
        }

        /// <summary>
        /// Gets Comment by Id
        /// </summary>
        /// <param name="id">Id of the Comment to get</param>
        /// <returns>Task with specified Comment</returns>
        public async Task<Comment> GetComment(int id)
        {
            var validator = new IdValidator();
            validator.ValidateAndThrow(id);

            Comment comment = await Comments.Get(id);
            return comment;
        }
    }
}