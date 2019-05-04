﻿using GreenShop.Web.Bff.Shopping.Api.Models.Comments;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenShop.Web.Bff.Shopping.Api.Services.Catalog.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetAllProductComments(int productID);

        Task<Comment> GetComment(int id);

        Task<int> AddComment(Comment comment);

        Task<bool> EditComment(int id, string message);

        Task<bool> EditComment(Comment comment);

        Task<bool> DeleteComment(int id);
    }
}