using GreenShop.Web.Bff.Shopping.Models.Comments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenShop.Web.Bff.Shopping.Services.Catalog.Interfaces
{
    public interface ICommentConsumer : IConsumer<Comment>
    {
        Task<IEnumerable<Comment>> GetAllProductRelatedCommentsAsync(int productId);
        Task<bool> EditAsync(int id, string message);
    }
}
