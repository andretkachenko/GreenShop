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
        public readonly IDataAccessor<Comment> Comments;

        public CommentsRepository(IDataAccessor<Comment> dataAccessor)
        {
            Comments = dataAccessor;
        }

        public async Task<bool> AddComment(Comment comment)
        {
            return await Comments.Add(comment) == 1;
        }
       
        public async Task<bool> DeleteComment(int id)
        {
            var validator = new CommentIdValidator();
            validator.ValidateAndThrow(id);

            return await Comments.Delete(id) == 1;
        }

        public async Task<bool> EditComment(Comment comment)
        {
            return await Comments.Edit(comment) == 1;
        }

        public async Task<IEnumerable<Comment>> GetAllCommetns()
        {
            return await Comments.GetAll();
        }

        public async Task<Comment> GetComment(int id)
        {
            return await Comments.Get(id);
        }
    }
}
