using GreenShop.Catalog.Api.Service.Products;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Api.Controllers
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
        public async Task<ActionResult<bool>> EditComment(int id, [FromBody] string message)
        {
            try
            {
                bool success = await _productsService.EditCommentAsync(id, message);
                return Ok(success);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // DELETE api/comment/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteCommentAsync(int id)
        {
            try
            {
                bool success = await _productsService.DeleteCommentAsync(id);
                return Ok(success);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // POST api/comment
        [HttpPost]
        public async Task<ActionResult<int>> AddCommentAsync([FromBody] CommentDto comment)
        {
            try
            {
                int id = await _productsService.AddCommentAsync(comment);
                return Ok(id);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}