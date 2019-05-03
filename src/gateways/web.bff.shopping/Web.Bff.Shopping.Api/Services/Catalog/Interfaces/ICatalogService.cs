using GreenShop.Web.Bff.Shopping.Api.Models.Categories;
using GreenShop.Web.Bff.Shopping.Api.Models.Comments;
using GreenShop.Web.Bff.Shopping.Api.Models.DTO;
using GreenShop.Web.Bff.Shopping.Api.Models.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenShop.Web.Bff.Shopping.Api.Services.Catalog.Interfaces
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

        Task<IEnumerable<Comment>> GetAllProductCommentsAsync(int productID);
        Task<Comment> GetCommentAsync(int id);
        Task<int> AddCommentAsync(Comment comment);
        Task<bool> EditCommentAsync(int id, string message);
        Task<bool> EditCommentAsync(Comment comment);
        Task<bool> DeleteCommentAsync(int id);
    }
}
