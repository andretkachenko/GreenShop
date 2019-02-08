using Catalog.Services.Comments.Interfaces;
using Common.Interfaces;
using Common.Models.Comments;
using Common.Validatiors.Comments;
using FluentValidation;
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

        public async Task<bool> AddComment(Comment comment)
        {
            var validator = new CommentValidator();
            validator.ValidateAndThrow(comment);

            var rowsAffected = await Comments.Add(comment);
            var resul = rowsAffected == 1;
            return resul;
        }

        public async Task<bool> DeleteComment(int id)
        {
            var validator = new CommentIdValidator();
            validator.ValidateAndThrow(id);

            var rowsAffected = await Comments.Delete(id);
            var result = rowsAffected == 1;
            return result;
        }

        public async Task<bool> EditComment(Comment comment)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> EditComment(int id, string message)
        {
            var idValidator = new CommentIdValidator();
            idValidator.ValidateAndThrow(id);

            var messageValidator = new CommenMessageValidator();
            messageValidator.ValidateAndThrow(message);

            var rowsAffected = await Comments.Edit(id, message);
            var result = rowsAffected == 1;
            return result;
        }

        public async Task<IEnumerable<Comment>> GetAllProductComments(int productID)
        {
            var comments = await Comments.GetAllParentRelated(productID);
            return comments;
        }

        public async Task<Comment> GetComment(int id)
        {
            var validator = new CommentIdValidator();
            validator.ValidateAndThrow(id);

            Comment comment = await Comments.Get(id);
            return comment;
        }
    }
}