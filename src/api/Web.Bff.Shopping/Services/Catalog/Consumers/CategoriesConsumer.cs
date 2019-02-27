using Web.Bff.Shopping.Services.Catalog.Interfaces;
using Common.Models.Categories;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Web.Bff.Shopping.Config;
using RestSharp;
using Web.Bff.Shopping.Properties;
using Web.Bff.Shopping.Helpers;
using System.Threading.Tasks;

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
            IRestResponse<List<Category>> response = await ExecuteAsync<List<Category>>(request);
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
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.CategoryApiOperations.GetAllCategories, Method.GET);
            IRestResponse<Category> response = await ExecuteAsync<Category>(request);
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
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.CategoryApiOperations.GetAllCategories, Method.GET, category);
            IRestResponse<int> response = await ExecuteAsync<int>(request);
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
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.CategoryApiOperations.GetAllCategories, Method.GET, category);
            IRestResponse<bool> response = await ExecuteAsync<bool>(request);
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
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.CategoryApiOperations.GetAllCategories, Method.GET);
            IRestResponse<bool> response = await ExecuteAsync<bool>(request);
            bool result = response.Data;
            return result;
        }

        /// <summary>
        /// Asynchronously execute request using the consumer's client
        /// </summary>
        /// <typeparam name="T">Response object type</typeparam>
        /// <param name="request">Request to execute</param>
        /// <returns>Response, returned by API</returns>
        private async Task<IRestResponse<T>> ExecuteAsync<T>(RestRequest request) where T : new()
        {
            var taskCompletionSource = new TaskCompletionSource<IRestResponse<T>>();
            _client.ExecuteAsync<T>(request, restResponse =>
        {
                if (restResponse.ErrorException != null)
                {
                    throw new ApplicationException(Resources.RestSharpException, restResponse.ErrorException);
                }
                taskCompletionSource.SetResult(restResponse);
            });

            return await taskCompletionSource.Task;
        }
    }
}
