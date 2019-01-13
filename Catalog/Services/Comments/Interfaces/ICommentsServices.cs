using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Models.Comments;

namespace Catalog.Services.Comments.Interfaces
{
    public interface ICommentsService
    {
        Task<IEnumerable<Comment>> GetAllCommetns();

        Task<Comment> GetComment(int id);

        Task<bool> AddComment(Comment comment);

        Task<bool> EditComment(Comment comment);

        Task<bool> DeleteComment(int id);
    }
}
