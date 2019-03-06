using RestSharp;
using System.Threading.Tasks;

namespace Web.Bff.Shopping.Extensions
{
    public static class IRestClientExtensions
    {
        /// <summary>
        /// Asynchronously execute request using the consumer's client
        /// </summary>
        /// <typeparam name="T">Response object type</typeparam>
        /// <param name="request">Request to execute</param>
        /// <returns>Response, returned by API</returns>
        public static async Task<IRestResponse<T>> ExecuteAsync<T>(this IRestClient restClient, RestRequest request) where T : new()
        {
            TaskCompletionSource<IRestResponse<T>> taskCompletionSource = new TaskCompletionSource<IRestResponse<T>>();
            restClient.ExecuteAsync<T>(request, restResponse =>
            {
                taskCompletionSource.SetResult(restResponse);
            });

            return await taskCompletionSource.Task;
        }
    }
}
