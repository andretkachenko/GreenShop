using GreenShop.Web.Mvc.App.Models.Categories;
using GreenShop.Web.Mvc.App.Models.Comments;
using GreenShop.Web.Mvc.App.Models.DTO;
using GreenShop.Web.Mvc.App.Models.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenShop.Web.Mvc.App.Services.Interfaces
{
    public interface ICatalogConsumer
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

        Task<IEnumerable<Comment>> GetallProductCommentsAsync(int productId);
        Task<Comment> GetCommentAsync(int id);
        Task<int> AddCommentAsync(Comment comment);
        Task<bool> EditCommentAsync(int id, string message);
        Task<bool> DeleteCommentAsync(int id);
    }
}
