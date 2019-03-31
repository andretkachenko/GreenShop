using FluentValidation;
using GreenShop.Catalog.DataAccessors.Interfaces;
using GreenShop.Catalog.Models.Comments;
using GreenShop.Catalog.Services.Comments.Interfaces;
using GreenShop.Catalog.Validators;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Services.Comments
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
        public async Task<int> AddComment(Comment comment)
        {
            CommentValidator validator = new CommentValidator();
            validator.ValidateAndThrow(comment);

            EntityNameValidator stringValidator = new EntityNameValidator();
            stringValidator.ValidateAndThrow(comment.Message);

            int id = await Comments.Add(comment);
            return id;
        }

        /// <summary>
        /// Asynchronously Deletes Comment by Id
        /// </summary>
        /// <param name="id">Id of the Comment to delete</param>
        /// <returns>True if succeeded</returns>
        public async Task<bool> DeleteComment(int id)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(id);

            int rowsAffected = await Comments.Delete(id);
            bool result = rowsAffected == 1;
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
            IdValidator idValidator = new IdValidator();
            idValidator.ValidateAndThrow(id);

            CommentMessageValidator messageValidator = new CommentMessageValidator();
            messageValidator.ValidateAndThrow(message);

            int rowsAffected = await Comments.Edit(id, message);
            bool result = rowsAffected == 1;
            return result;
        }

        /// <summary>
        /// Gets all Comments by Product Id
        /// </summary>
        /// <param name="productId">Id of the product to get its comments</param>
        /// <returns>Task with list of all comments</returns>
        public async Task<IEnumerable<Comment>> GetAllProductComments(int productId)
        {
            IdValidator idValidator = new IdValidator();
            idValidator.ValidateAndThrow(productId);

            IEnumerable<Comment> comments = await Comments.GetAllParentRelated(productId);
            return comments;
        }

        /// <summary>
        /// Gets Comment by Id
        /// </summary>
        /// <param name="id">Id of the Comment to get</param>
        /// <returns>Task with specified Comment</returns>
        public async Task<Comment> GetComment(int id)
        {
            IdValidator validator = new IdValidator();
            validator.ValidateAndThrow(id);

            Comment comment = await Comments.Get(id);
            return comment;
        }
    }
}