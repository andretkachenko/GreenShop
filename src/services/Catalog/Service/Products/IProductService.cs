using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Service.Products
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto> GetAsync(int id);
        Task<int> CreateAsync(ProductDto product);
        Task<bool> UpdateAsync(ProductDto product);
        Task<bool> DeleteAsync(int id);

        Task<int> AddCommentAsync(CommentDto commentDto);
        Task<bool> DeleteCommentAsync(int id);
        Task<bool> EditComment(int id, string message);
    }
}
