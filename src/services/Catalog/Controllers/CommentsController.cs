using GreenShop.Catalog.Models.Comments;
using GreenShop.Catalog.Services.Comments.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Controllers
{
    [ApiVersion("1")]
    [Produces("application/json")]
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
        [HttpGet("product/{productId}")]
        public async Task<IEnumerable<Comment>> GetAllProductComments(int productId)
        {
            IEnumerable<Comment> comments = await _commentServices.GetAllProductComments(productId);

            return comments;
        }

        //Get comment by ID
        [HttpGet("{id}")]
        public async Task<Comment> GetComment(int id)
        {
            Comment comment = await _commentServices.GetComment(id);

            return comment;
        }

        //Put Comment
        [HttpPut("{id}")]
        public async Task<bool> EditComment(int id, [FromBody] string message)
        {
            bool success = await _commentServices.EditComment(id, message);

            return success;
        }

        //Delete Comment
        [HttpDelete("{id}")]
        public async Task<bool> DeleteComment(int id)
        {
            bool success = await _commentServices.DeleteComment(id);

            return success;
        }

        //Post Comment
        [HttpPost]
        public async Task<int> AddComment([FromBody] Comment comment)
        {
            int id = await _commentServices.AddComment(comment);

            return id;
        }
    }
}