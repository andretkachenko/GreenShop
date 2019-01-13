﻿using Catalog.Services.Comments.Interfaces;
using Common.Interfaces;
using Common.Models.Comments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Services.Comments
{
    public class CommentsService : ICommentsService
    {
        public readonly IDataAccessor<Comment> Comments;

        public CommentsService(IDataAccessor<Comment> dataAccessor)
        {
            Comments = dataAccessor;
        }

        public async Task<bool> AddComment(Comment comment)
        {
            return await Comments.Add(comment) == 1;
        }

        public async Task<bool> DeleteComment(int id)
        {
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
