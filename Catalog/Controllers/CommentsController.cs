using Microsoft.AspNetCore.Mvc;
using Common.Models.Comments;
using Catalog.Services.Comments.Interfaces;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

namespace Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsRepository _commentServices;

        public CommentsController(ICommentsRepository commentsRepository)
        {
            _commentServices = commentsRepository;
        }

        //GET all comments from product
        [HttpGet("{productID}")]
        public async Task<IEnumerable<Comment>> GetAllProductCommentsAsync(int productID)
        {
            var comments = await _commentServices.GetAllProductComments(productID);

            return comments;
        }

        //Get comment by ID
        [HttpGet("{productID}/comments/{id})")]
        public async Task<Comment> GetComment(int id)
        {
            var comment = await _commentServices.GetComment(id);

            return comment;
        }

        //Put Comment
        [HttpPut("{productID}")]
        public async Task<bool> EditCommentAsync([FromBody] Comment comment)
        {
            var success = await _commentServices.EditComment(comment);

            return success;
        }

        //Delete Comment
        [HttpDelete("{productID}/comments/{id})")]
        public async Task<bool> DeleteComment(int id)
        {
            var success = await _commentServices.DeleteComment(id);

            return success;
        }

        //Post Comment
        [HttpPost("{productID}")]
        public async Task<bool> AddComment([FromBody] Comment comment)
        {
            var success = await _commentServices.AddComment(comment);

            return success;
        }
    }
}