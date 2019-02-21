using Web.Bff.Shopping.Services.Catalog.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Bff.Shopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }
    }
}
