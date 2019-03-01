using Common.Models.Products;
using Microsoft.Extensions.Options;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Bff.Shopping.Config;
using Web.Bff.Shopping.Helpers;
using Web.Bff.Shopping.Services.Catalog.Interfaces;

namespace Web.Bff.Shopping.Services.Catalog.Consumers
{
    public class ProductsConsumer : IConsumer<Product>
    {
        private readonly UrlsConfig _urls;
        private readonly IRestClient _client;

        public ProductsConsumer(IOptionsSnapshot<UrlsConfig> config)
        {
            _urls = config.Value;
            _client = new RestClient(_urls.Catalog);
        }

        /// <summary>
        /// Asynchronously get all Products
        /// </summary>
        /// <returns>List of all Products</returns>
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.ProductApiOperations.GetAllProducts, Method.GET);
            IRestResponse<List<Product>> response = await ExecuteAsync<List<Product>>(request);
            List<Product> products = response.Data;
            return products;
        }

        /// <summary>
        /// Asynchronously get Product with the specific id
        /// </summary>
        /// <param name="id">Id of the Product to get</param>
        /// <returns>Specified Product</returns>
        public async Task<Product> GetAsync(int id)
        {
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.ProductApiOperations.GetProduct(id), Method.GET);
            IRestResponse<Product> response = await ExecuteAsync<Product>(request);
            Product product = response.Data;
            return product;
        }

        /// <summary>
        /// Asynchronously add Product
        /// </summary>
        /// <param name="Product">Product to add</param>
        /// <returns>Product id</returns>
        public async Task<int> AddAsync(Product Product)
        {
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.ProductApiOperations.AddProduct, Method.POST, Product);
            IRestResponse<int> response = await ExecuteAsync<int>(request);
            int id = response.Data;
            return id;
        }

        /// <summary>
        /// Asynchronously edit specified Product
        /// </summary>
        /// <param name="Product">Product, that contains id of entity that should be changed, and all changed values</param>
        /// <returns>Operation result</returns>
        public async Task<bool> EditAsync(Product Product)
        {
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.ProductApiOperations.EditProduct, Method.PUT, Product);
            IRestResponse<bool> response = await ExecuteAsync<bool>(request);
            bool result = response.Data;
            return result;
        }

        /// <summary>
        /// Asynchronously remove Product with specified id
        /// </summary>
        /// <param name="id">Id of the Product to delete</param>
        /// <returns>Operation result</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.ProductApiOperations.DeleteProduct(id), Method.DELETE);
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
            TaskCompletionSource<IRestResponse<T>> taskCompletionSource = new TaskCompletionSource<IRestResponse<T>>();
            _client.ExecuteAsync<T>(request, restResponse =>
            {
                taskCompletionSource.SetResult(restResponse);
            });

            return await taskCompletionSource.Task;
        }
    }
}
