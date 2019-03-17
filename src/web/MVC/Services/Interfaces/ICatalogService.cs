using Common.Models.Categories;
using Common.Models.Comments;
using Common.Models.DTO;
using Common.Models.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenShop.MVC.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryAsync(int id);
        Task<CategoryProductsDTO> GetCategoryWithProductsAsync(int id);
        Task<int> AddCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(int id);
        Task<bool> EditCategoryAsync(Category category);

        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductAsync(int id);
        Task<Product> GetProductWithCategoryAsync(int id);
        Task<int> AddProductAsync(Product product);
        Task<bool> DeleteProductAsync(int id);
        Task<bool> EditProductAsync(Product product);

        Task<IEnumerable<Comment>> GetAllProductComments(int productId);
        Task<Comment> GetComment(int id);
        Task<int> AddComment(Comment comment);
        Task<bool> EditComment(Comment comment);
        Task<bool> DeleteComment(int id);
    }
}
