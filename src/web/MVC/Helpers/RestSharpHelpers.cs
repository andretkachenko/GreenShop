using RestSharp;

namespace GreenShop.MVC.Helpers
{
    public static class RestSharpHelpers
    {
        /// <summary>
        /// Create Rest Request with the specified url appendix, set HTTP Method of the request, and, if needed - inserts body into the request
        /// </summary>
        /// <param name="resource">Url part, that should be added to the basic url in Rest Client</param>
        /// <param name="httpMethod">HTTP method, expected by the API</param>
        /// <param name="jsonBody">Body of the response</param>
        /// <returns>Assembled RestRequest</returns>
        public static RestRequest AssembleRestRequest(string resource, Method httpMethod, object jsonBody = null)
        {
            IRestRequest request = new RestRequest(resource, httpMethod);
            if (jsonBody != null) request.AddJsonBody(jsonBody);
            return request as RestRequest;
        }
    }
}
