using GreenShop.Web.Bff.Shopping.Api.Models.Comments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenShop.Web.Bff.Shopping.Api.Services.Catalog.Interfaces
{
    public interface ICommentsConsumer : IConsumer<Comment>
    {
        Task<IEnumerable<Comment>> GetAllProductRelatedCommentsAsync(int productId);
        Task<bool> EditAsync(int id, string message);
    }
}
