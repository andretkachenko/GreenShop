using ApiGateway.Services.Catalog.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebShoppingController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        public WebShoppingController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }
    }
}
