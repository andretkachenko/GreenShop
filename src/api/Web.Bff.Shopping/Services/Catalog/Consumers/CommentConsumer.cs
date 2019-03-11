using Common.Models.Comments;
using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Bff.Shopping.Config;
using Web.Bff.Shopping.Extensions;
using Web.Bff.Shopping.Helpers;
using Web.Bff.Shopping.Services.Catalog.Interfaces;

namespace Web.Bff.Shopping.Services.Catalog.Consumers
{
    public class CommentConsumer : ICommentConsumer
    {
        private readonly UrlsConfig _urls;
        private readonly IRestClient _client;

        public CommentConsumer(IOptionsSnapshot<UrlsConfig> config)
        {
            _urls = config.Value;
            _client = new RestClient(_urls.Catalog);
        }

        /// <summary>
        ///Asynchronously Add Comment 
        /// </summary>
        /// <param name="comment">Comment to Add</param>
        /// <returns>Task with Comment Id</returns>
        public async Task<int> AddAsync(Comment comment)
        {
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.CommentApiOperations.AddComment, Method.POST, comment);
            IRestResponse<int> response = await _client.ExecuteAsync<int>(request);
            int id = response.Data;
            return id;
        }

        /// <summary>
        /// Asynchronously Delete Comment 
        /// </summary>
        /// <param name="id">Id of Comment to Delete</param>
        /// <returns>Task with boolean result</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.CommentApiOperations.DeleteComment(id), Method.DELETE);
            IRestResponse<bool> response = await _client.ExecuteAsync<bool>(request);
            bool result = response.Data;

            return result;
        }

        /// <summary>
        /// Asynchronously Edit comment's message
        /// <para>This method calls EditAsync(int, string) using Id and Message from the Comment</para>
        /// </summary>
        /// <param name="comment">Comment to edit</param>
        /// <returns>True if succeeded</returns>
        public async Task<bool> EditAsync(Comment comment) => await EditAsync(comment.Id, comment.Message);

        /// <summary>
        /// Asynchronously Edit Comment
        /// </summary>
        /// <param name="id">Id of Comment to update</param>
        /// <param name="message">New text for message</param>
        /// <returns>Task with boolean result</returns>
        public async Task<bool> EditAsync(int id, string message)
        {
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.CommentApiOperations.EditComment(id), Method.PUT, message);
            IRestResponse<bool> response = await _client.ExecuteAsync<bool>(request);
            bool result = response.Data;

            return result;
        }

        /// <summary>
        /// Asynchronously Get all comments for requested product 
        /// </summary>
        /// <param name="productId">Product Id for product</param>
        /// <returns>Task with list of comments</returns>
        public async Task<IEnumerable<Comment>> GetAllProductRelatedCommentsAsync(int productId)
        {
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.CommentApiOperations.GetAllProductComments(productId), Method.GET);
            IRestResponse<List<Comment>> response = await _client.ExecuteAsync<List<Comment>>(request);
            List<Comment> comment = response.Data;

            return comment;
        }

        /// <summary>
        /// For this time Not implemented
        /// </summary>
        /// <returns>NotImplementedException</returns>     
        public Task<IEnumerable<Comment>> GetAllAsync() => throw new NotImplementedException();

        /// <summary>
        /// Asynchronously get Comment by Id
        /// </summary>
        /// <param name="id">Id for requested Comment</param>
        /// <returns>Task with Comment</returns>
        public async Task<Comment> GetAsync(int id)
        {
            RestRequest request = RestSharpHelpers.AssembleRestRequest(UrlsConfig.CommentApiOperations.GetComment(id), Method.GET);
            IRestResponse<Comment> response = await _client.ExecuteAsync<Comment>(request);
            Comment comment = response.Data;

            return comment;
        }        
    }
}
