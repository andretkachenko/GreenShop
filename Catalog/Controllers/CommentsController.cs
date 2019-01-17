using Microsoft.AspNetCore.Mvc;
using Common.Models.Comments;
using Catalog.Services.Comments.Interfaces;

namespace Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsRepository _commentServices;
        public CommentsController()
        {

        }
    }
}