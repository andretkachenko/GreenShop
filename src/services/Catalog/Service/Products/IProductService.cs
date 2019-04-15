﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Service.Products
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto> GetAsync(string id);
        Task<Guid> CreateAsync(ProductDto product);
        Task<bool> UpdateAsync(ProductDto product);
        Task<bool> DeleteAsync(string id);

        Task<Guid> AddCommentAsync(CommentDto commentDto);
        Task<bool> DeleteCommentAsync(Guid id);
        Task<bool> EditComment(Guid id, string message);
    }
}
