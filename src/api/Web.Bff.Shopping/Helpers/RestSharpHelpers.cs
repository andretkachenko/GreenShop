using RestSharp;

namespace Web.Bff.Shopping.Helpers
{
    internal static class RestSharpHelpers
    {
        /// <summary>
        /// Create Rest Request with the specified url appendix, set HTTP Method of the request, and, if needed - inserts body into the request
        /// </summary>
        /// <param name="urlAppendix">Url part, that should be added to the basic url in Rest Client</param>
        /// <param name="httpMethod">HTTP method, expected by the API</param>
        /// <param name="jsonBody">Body of the response</param>
        /// <returns>Assembled RestRequest</returns>
        internal static RestRequest AssembleRestRequest(string urlAppendix, Method httpMethod, object jsonBody = null)
        {
            IRestRequest request = new RestRequest(urlAppendix, httpMethod);
            if (jsonBody != null) request.AddJsonBody(jsonBody);
            return request as RestRequest;
        }
    }
}
