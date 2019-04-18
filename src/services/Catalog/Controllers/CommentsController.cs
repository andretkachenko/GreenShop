using GreenShop.Catalog.Service.Products;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Controllers
{
    [ApiVersion("1")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IProductService _productsService;

        public CommentsController(IProductService productsService)
        {
            _productsService = productsService;
        }

        // PUT api/comment/5
        [HttpPut("{id}")]
        public async Task<bool> EditComment(int id, [FromBody] string message)
        {
            bool success = await _productsService.EditComment(id, message);

            return success;
        }

        // DELETE api/comment/5
        [HttpDelete("{id}")]
        public async Task<bool> DeleteComment(int id)
        {
            bool success = await _productsService.DeleteCommentAsync(id);

            return success;
        }

        // POST api/comment
        [HttpPost]
        public async Task<int> AddComment([FromBody] CommentDto comment)
        {
            int id = await _productsService.AddCommentAsync(comment);

            return id;
        }
    }
}