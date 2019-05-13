using GreenShop.Catalog.Api.Properties;
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

        /// <summary>
        /// Edit Comment's message
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT api/comments/5
        ///     {
        ///         "Longer longer story"
        ///     }
        /// 
        /// </remarks>
        /// <param name="id">Id of the Comment which should be changed</param>
        /// <param name="message">New desired message</param>
        /// <response code="200">Comment was updated successfully</response>
        /// <response code="404">Unable to found the Comment</response>  
        /// <response code="500">Internal Server Error occured while processing the request</response>      
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Consumes("application/json")]
        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> EditComment(int id, [FromBody] string message)
        {
            try
            {
                bool success = await _productsService.EditCommentAsync(id, message);

                if (success)
                    return Ok(success);
                else
                    return NotFound(string.Format(Resources.FailureResponse, Resources.Update, Resources.Comment, Resources.EntityNotFound));
            }
            catch (Exception e)
            {
                return StatusCode(500, string.Format(Resources.FailureResponse, Resources.Update, Resources.Comment, e.Message));
            }
        }

        /// <summary>
        /// Delete Comment with the specified Id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DELETE api/comments/5
        /// 
        /// </remarks>
        /// <param name="id">Id for the Comment that should be deleted</param>
        /// <response code="200">Comment was deleted successfully</response>
        /// <response code="404">Unable to found the Comment</response>  
        /// <response code="500">Internal Server Error occured while processing the request</response>       
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteCommentAsync(int id)
        {
            try
            {
                bool success = await _productsService.DeleteCommentAsync(id);

                if (success)
                    return Ok(success);
                else
                    return NotFound(string.Format(Resources.FailureResponse, Resources.Delete, Resources.Comment, Resources.EntityNotFound));
            }
            catch (Exception e)
            {
                return StatusCode(500, string.Format(Resources.FailureResponse, Resources.Delete, Resources.Comment, e.Message));
            }
        }


        /// <summary>
        /// Create a Comment
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST api/comments
        ///     {
        ///     "authorId": 1,
        ///     "message": "Long long story",
        ///     "productId": 1
        /// }
        /// 
        /// </remarks>
        /// <param name="comment">Comment with all necessary properties set</param>
        /// <returns>A newly created Comment</returns>
        /// <response code="201">Return Id of the newly created Comment</response>
        /// <response code="400">Unable to successfully process the Comment</response>    
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Consumes("application/json")]
        [HttpPost]
        public async Task<ActionResult<int>> AddCommentAsync([FromBody] CommentDto comment)
        {
            try
            {
                int id = await _productsService.AddCommentAsync(comment);
                // CreatedAtAction supplies response with the Location header, which will contain
                // route to get the Product
                // For example
                // Location → .../api/Products/4
                return CreatedAtAction("GetProductAsync", new { id }, comment.ProductId);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}