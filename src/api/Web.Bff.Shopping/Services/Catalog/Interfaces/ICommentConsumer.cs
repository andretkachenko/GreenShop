﻿using Common.Models.Comments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web.Bff.Shopping.Services.Catalog.Interfaces
{
    public interface ICommentConsumer : IConsumer<Comment>
    {
        Task<IEnumerable<Comment>> GetAllProductRelatedCommentsAsync(int productId);
        Task<bool> EditAsync(int id, string message);
    }
}