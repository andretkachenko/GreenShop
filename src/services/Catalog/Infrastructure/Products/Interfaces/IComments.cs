using GreenShop.Catalog.Models.Comments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Infrastructure.Products.Interfaces
{
    public interface IComments : IRepository<Comment>
    {
        Task<bool> UpdateAsync(Guid id, string message);
        Task<IEnumerable<Comment>> GetAllParentRelatedAsync(Guid productId);
    }
}
