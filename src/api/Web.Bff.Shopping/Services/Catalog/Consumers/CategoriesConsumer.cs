using Web.Bff.Shopping.Services.Catalog.Interfaces;
using Common.Models.Categories;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Web.Bff.Shopping.Config;
using RestSharp;
using Web.Bff.Shopping.Helpers;
using System.Threading.Tasks;
using Web.Bff.Shopping.Extensions;

namespace Web.Bff.Shopping.Services.Catalog.Consumers
{
    public class CategoriesConsumer : IConsumer<Category>
    {
        private readonly UrlsConfig _urls;
        private readonly IRestClient _client;

        public CategoriesConsumer(IOptionsSnapshot<UrlsConfig> config)
        {
            _urls = config.Value;
            _client = new RestClient(_urls.Catalog);
        }

        /// <summary>
        /// Asynchronously get all Categories
        /// </summary>
        /// <returns>List of all Categories</returns>
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.CategoryApiOperations.GetAllCategories, Method.GET);
            IRestResponse<List<Category>> response = await _client.ExecuteAsync<List<Category>>(request);
            List<Category> categories = response.Data;
            return categories;
        }

        /// <summary>
        /// Asynchronously get Category with the specific id
        /// </summary>
        /// <param name="id">Id of the Category to get</param>
        /// <returns>Specified Category</returns>
        public async Task<Category> GetAsync(int id)
        {
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.CategoryApiOperations.GetCategory(id), Method.GET);
            IRestResponse<Category> response = await _client.ExecuteAsync<Category>(request);
            Category category = response.Data;
            return category;
        }

        /// <summary>
        /// Asynchronously add Category
        /// </summary>
        /// <param name="category">Category to add</param>
        /// <returns>Category id</returns>
        public async Task<int> AddAsync(Category category)
        {
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.CategoryApiOperations.AddCategory, Method.POST, category);
            IRestResponse<int> response = await _client.ExecuteAsync<int>(request);
            int id = response.Data;
            return id;
        }

        /// <summary>
        /// Asynchronously edit specified Category
        /// </summary>
        /// <param name="category">Category, that contains id of entity that should be changed, and all changed values</param>
        /// <returns>Operation result</returns>
        public async Task<bool> EditAsync(Category category)
        {
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.CategoryApiOperations.EditCategory, Method.PUT, category);
            IRestResponse<bool> response = await _client.ExecuteAsync<bool>(request);
            bool result = response.Data;
            return result;
        }

        /// <summary>
        /// Asynchronously remove Category with specified id
        /// </summary>
        /// <param name="id">Id of the Category to delete</param>
        /// <returns>Operation result</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.CategoryApiOperations.DeleteCategory(id), Method.DELETE);
            IRestResponse<bool> response = await _client.ExecuteAsync<bool>(request);
            bool result = response.Data;
            return result;
        }
    }
}
