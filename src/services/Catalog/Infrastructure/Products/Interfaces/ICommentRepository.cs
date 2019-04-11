using GreenShop.Catalog.Models.Comments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Infrastructure.Products.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        /// <summary>
        /// Asynchronously add List of the Comments
        /// </summary>
        /// <param name="comment">List of the Comments to insert to the database</param>
        /// <returns>Task with summary result for all list</returns>
        Task<bool> CreateAsync(IEnumerable<Comment> comments);

        /// <summary>
        /// Asynchronously Edit comment's message
        /// </summary>
        /// <param name="id">Id of the Comment to edit</param>
        /// <param name="message">Updated message for the Comment</param>
        /// <returns>Task with number of proceeded rows</returns>
        Task<bool> UpdateAsync(Guid id, string message);

        /// <summary>
        /// Asynchronously Get all Comments by product ID
        /// </summary>
        /// <param name="productId">Id of the product to get its comments</param>
        /// <returns>Task with list of comments</returns>
        Task<IEnumerable<Comment>> GetAllParentRelatedAsync(Guid productId);

        /// <summary>
        /// Asynchronously Get all Comments for the Products, which IDs are presented in the list
        /// </summary>
        /// <param name="productId">List of Ids of the products</param>
        /// <returns>Task with list of comments</returns>
        Task<Dictionary<Guid, IEnumerable<Comment>>> GetAllParentRelatedAsync(IEnumerable<Guid> productIds);

        /// <summary>
        /// Asynchronously Delete all Comments for the specified Product
        /// </summary>
        /// <param name="productIds">Product Id</param>
        /// <returns>Result flag</returns>
        Task<bool> DeleteAllParentRelatedAsync(Guid productId);
    }
}
