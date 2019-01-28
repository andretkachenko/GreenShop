using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Models.Comments;

namespace Catalog.Services.Comments.Interfaces
{
    public interface ICommentsRepository
    {
        Task<IEnumerable<Comment>> GetAllProductComments(int productID);

        Task<Comment> GetComment(int id);

        Task<bool> AddComment(Comment comment);

        Task<bool> EditComment(Comment comment);

        Task<bool> DeleteComment(int id);
    }
}
