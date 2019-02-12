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
        [HttpGet("{productId}")]
        public async Task<IEnumerable<Comment>> GetAllProductCommentsAsync(int productId)
        {
            var comments = await _commentServices.GetAllProductComments(productId);

            return comments;
        }

        //Get comment by ID
        [HttpGet("{id}")]
        public async Task<Comment> GetComment(int id)
        {
            var comment = await _commentServices.GetComment(id);

            return comment;
        }

        //Put Comment
        [HttpPut("{productId}")]
        public async Task<bool> EditCommentAsync(int productId, [FromBody] string message)
        {
            var success = await _commentServices.EditComment(productId, message);

            return success;
        }

        //Delete Comment
        [HttpDelete("{id}")]
        public async Task<bool> DeleteComment(int id)
        {
            var success = await _commentServices.DeleteComment(id);

            return success;
        }

        //Post Comment
        [HttpPost]
        public async Task<bool> AddComment([FromBody] Comment comment)
        {
            var success = await _commentServices.AddComment(comment);

            return success;
        }
    }
}