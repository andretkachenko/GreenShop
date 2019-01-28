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
        public readonly IChildtDataAccessor<Comment> Comments;

        public CommentsRepository(IChildtDataAccessor<Comment> dataAccessor)
        {
            Comments = dataAccessor;
        }

        public async Task<bool> AddComment(int productID, Comment comment)
        {
            var validator = new CommentValidator();
            validator.ValidateAndThrow(comment);

            var rowEffected = await Comments.Add(productID, comment);
            var resul = rowEffected == 1;
            return resul;
        }

        public async Task<bool> DeleteComment(int productID, int id)
        {
            var validator = new CommentIdValidator();
            validator.ValidateAndThrow(id);

            var rowEffected = await Comments.Delete(productID, id);
            var result = rowEffected == 1;
            return result;
        }

        public async Task<bool> EditComment(int productID, Comment comment)
        {
            var validator = new CommentValidator();
            validator.ValidateAndThrow(comment);

            var rowEffected = await Comments.Edit(productID, comment);
            var result = rowEffected == 1;
            return result;
        }

        public async Task<IEnumerable<Comment>> GetAllCommetns(int productID)
        {
            var comments = await Comments.GetAll(productID);
            return comments;
        }

        public async Task<Comment> GetComment(int productID, int id)
        {
            var validator = new CommentIdValidator();
            validator.ValidateAndThrow(id);

            Comment comment = await Comments.Get(productID,id);
            return comment;
        }
    }
}