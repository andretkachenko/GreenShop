using ApiGateway.Services.Catalog.Interfaces;
using Common.Models.Comments;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Web.Bff.Shopping.Services.Catalog.Interfaces;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService _catalogService;
        private readonly ICommentService _commentService;

        public CatalogController(ICatalogService catalogService, ICommentService commentService)
        {
            _catalogService = catalogService;
            _commentService = commentService;
        }

        [HttpGet("{id}")]
        public async Task<Comment> GetComment(int id)
        {
            var result = await _commentService.GetComment(id);

            return result;
        }
    }
}
