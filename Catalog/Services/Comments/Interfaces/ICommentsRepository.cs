using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Models.Comments;

namespace Catalog.Services.Comments.Interfaces
{
    public interface ICommentsRepository
    {
        Task<IEnumerable<Comment>> GetAllCommetns(int id);

        Task<Comment> GetComment(int productID, int id);

        Task<bool> AddComment(int productID, Comment comment);

        Task<bool> EditComment(int productID, Comment comment);

        Task<bool> DeleteComment(int productID, int id);
    }
}
