using ApiGateway.Config;
using Common.Models.Comments;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Bff.Shopping.Services.Catalog.Interfaces;

namespace Web.Bff.Shopping.Services.Catalog
{
    public class CommentService : ICommentService
    {
        public Task<int> AddComment(Comment comment)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteComment(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> EditComment(int id, string message)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Comment>> GetAllProductComments(int productID)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Comment> GetComment(int id)
        {
            var client = new RestClient();
            var request = new RestRequest(UrlsConfig.CatalogOperations.GetCategory(id), Method.GET, DataFormat.Json);
            IRestResponse<Comment> response = await client.ExecuteTaskAsync<Comment>(request);

            return response.Data;
        }

        private void GetComment(IRestResponse<Comment> arg1, RestRequestAsyncHandle arg2)
        {
            throw new NotImplementedException();
        }
    }
}
