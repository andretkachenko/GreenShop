using Common.Models.Categories;
using Common.Models.Comments;
using Common.Models.DTO;
using Common.Models.Products;
using GreenShop.MVC.Config;
using GreenShop.MVC.Extensions;
using GreenShop.MVC.Helpers;
using GreenShop.MVC.Services.Interfaces;
using Microsoft.Extensions.Options;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenShop.MVC.Services
{
    public class CatalogConsumer : ICatalogConsumer
    {
        private readonly UrlsConfig _urls;
        private readonly IRestClient _client;

        public CatalogConsumer(IOptionsSnapshot<UrlsConfig> config)
        {
            _urls = config.Value;
            _client = new RestClient(_urls.WebShoppingApi);
        }
        /// <summary>
        /// Asynchronously add Category
        /// </summary>
        /// <param name="category">Category to add</param>
        /// <returns>Category id</returns>
        public async Task<int> AddCategoryAsync(Category category)
        {
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.WebShoppingApiOperations.CategoryApiOperations.AddCategory, Method.POST, category);
            IRestResponse<int> response = await _client.ExecuteAsync<int>(request);
            int id = response.Data;
            return id;
        }

        /// <summary>
        /// Asynchronously add Product
        /// </summary>
        /// <param name="Product">Product to add</param>
        /// <returns>Product id</returns>
        public async Task<int> AddProductAsync(Product product)
        {
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.WebShoppingApiOperations.ProductApiOperations.AddProduct, Method.POST);
            IRestResponse<int> response = await _client.ExecuteAsync<int>(request);
            int id = response.Data;
            return id;
        }

        /// <summary>
        /// Asynchronously remove Category with specified id
        /// </summary>
        /// <param name="id">Id of the Category to delete</param>
        /// <returns>Operation result</returns>
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.WebShoppingApiOperations.CategoryApiOperations.DeleteCategory(id), Method.DELETE);
            IRestResponse<bool> response = await _client.ExecuteAsync<bool>(request);
            bool result = response.Data;
            return result;
        }

        /// <summary>
        /// Asynchronously remove Product with specified id
        /// </summary>
        /// <param name="id">Id of the Product to delete</param>
        /// <returns>Operation result</returns>
        public async Task<bool> DeleteProductAsync(int id)
        {
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.WebShoppingApiOperations.ProductApiOperations.DeleteProduct(id), Method.DELETE);
            IRestResponse<bool> response = await _client.ExecuteAsync<bool>(request);
            bool result = response.Data;
            return result;
        }

        /// <summary>
        /// Asynchronously edit specified Category
        /// </summary>
        /// <param name="category">Category, that contains id of entity that should be changed, and all changed values</param>
        /// <returns>Operation result</returns>
        public async Task<bool> EditCategoryAsync(Category category)
        {
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.WebShoppingApiOperations.CategoryApiOperations.EditCategory, Method.PUT, category);
            IRestResponse<bool> response = await _client.ExecuteAsync<bool>(request);
            bool result = response.Data;
            return result;
        }

        /// <summary>
        /// Asynchronously edit specified Product
        /// </summary>
        /// <param name="Product">Product, that contains id of entity that should be changed, and all changed values</param>
        /// <returns>Operation result</returns>
        public async Task<bool> EditProductAsync(Product product)
        {
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.WebShoppingApiOperations.ProductApiOperations.EditProduct, Method.PUT);
            IRestResponse<bool> response = await _client.ExecuteAsync<bool>(request);
            bool result = response.Data;
            return result;
        }

        /// <summary>
        /// Asynchronously get all Categories
        /// </summary>
        /// <returns>List of all Categories</returns>
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.WebShoppingApiOperations.CategoryApiOperations.GetAllCategories, Method.GET);
            IRestResponse<List<Category>> response = await _client.ExecuteAsync<List<Category>>(request);
            List<Category> categories = response.Data;
            return categories;
        }

        /// <summary>
        /// Asynchronously get all Products
        /// </summary>
        /// <returns>List of all Products</returns>
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.WebShoppingApiOperations.ProductApiOperations.GetAllProducts, Method.GET);
            IRestResponse<List<Product>> response = await _client.ExecuteAsync<List<Product>>(request);
            List<Product> products = response.Data;
            return products;
        }

        /// <summary>
        /// Asynchronously get Category with the specific id
        /// </summary>
        /// <param name="id">Id of the Category to get</param>
        public async Task<Category> GetCategoryAsync(int id)
        {
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.WebShoppingApiOperations.CategoryApiOperations.GetCategory(id), Method.GET);
            IRestResponse<Category> response = await _client.ExecuteAsync<Category>(request);
            Category category = response.Data;
            return category;
        }

        /// <summary>
        /// Asynchronously get Category with the specific id and Products which are connected to the specified Category
        /// </summary>
        /// <param name="id">Id of the Category to get</param>
        /// <returns>Specified Category and list of related Products</returns>
        public async Task<CategoryProductsDTO> GetCategoryWithProductsAsync(int id)
        {
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.WebShoppingApiOperations.CategoryApiOperations.GetCategoryWithRelatedProducts(id), Method.GET);
            IRestResponse<CategoryProductsDTO> response = await _client.ExecuteAsync<CategoryProductsDTO>(request);
            CategoryProductsDTO result = response.Data;
            return result;
        }

        /// <summary>
        /// Asynchronously get Product with the specific id
        /// </summary>
        /// <param name="id">Id of the Product to get</param>
        /// <returns>Specified Product</returns>
        public async Task<Product> GetProductAsync(int id)
        {
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.WebShoppingApiOperations.ProductApiOperations.GetProduct(id), Method.GET);
            IRestResponse<Product> response = await _client.ExecuteAsync<Product>(request);
            Product product = response.Data;
            return product;
        }

        /// <summary>
        /// Asynchronously get Product with the specific id with the related Category
        /// </summary>
        /// <param name="id">Id of the Product to get</param>
        /// <returns>Specified Product with the related Category</returns>
        public async Task<Product> GetProductWithCategoryAsync(int id)
        {
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.WebShoppingApiOperations.ProductApiOperations.GetProductWithCategory(id), Method.GET);
            IRestResponse<Product> response = await _client.ExecuteAsync<Product>(request);
            Product result = response.Data;
            return result;
        }

        /// <summary>
        /// Asynchronously get all product related comments by specified product Id 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>Collection of comments</returns>
        public async Task<IEnumerable<Comment>> GetallProductCommentsAsync(int productId)
        {
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.WebShoppingApiOperations.CommentApiOperations.GetAllProductComments(productId), Method.GET);
            IRestResponse<List<Comment>> response = await _client.ExecuteAsync<List<Comment>>(request);

            List<Comment> comment = response.Data;
            return comment;
        }

        /// <summary>
        /// Asynchronously get Comment by specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Comment</returns>
        public async Task<Comment> GetCommentAsync(int id)
        {
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.WebShoppingApiOperations.CommentApiOperations.GetComment(id), Method.GET);
            IRestResponse<Comment> response = await _client.ExecuteAsync<Comment>(request);

            Comment comment = response.Data;
            return comment;
        }

        /// <summary>
        /// Asynchronously add Comment
        /// </summary>
        /// <param name="comment"></param>
        /// <returns>Comment id</returns>
        public async Task<int> AddCommentAsync(Comment comment)
        {
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.WebShoppingApiOperations.CommentApiOperations.AddComment, Method.POST, comment);
            IRestResponse<int> response = await _client.ExecuteAsync<int>(request);

            int result = response.Data;
            return result;
        }

        /// <summary>
        /// Asynchronously edit specified Comment
        /// </summary>
        /// <param name="comment"></param>
        /// <returns>Boolean result</returns>
        public async Task<bool> EditCommentAsync(int id, string message)
        {
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.WebShoppingApiOperations.CommentApiOperations.EditComment(id), Method.PUT, message);
            IRestResponse<bool> response = await _client.ExecuteAsync<bool>(request);

            bool result = response.Data;
            return result;
        }

        /// <summary>
        /// Asynchronously delete Comment by specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Boolean result</returns>
        public async Task<bool> DeleteCommentAsync(int id)
        {
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.WebShoppingApiOperations.CommentApiOperations.DeleteComment(id), Method.POST);
            IRestResponse<bool> response = await _client.ExecuteAsync<bool>(request);

            bool result = response.Data;
            return result;
        }
    }
}
