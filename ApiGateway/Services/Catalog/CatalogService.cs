using ApiGateway.Config;
using ApiGateway.Services.Catalog.Interfaces;
using Common.Models.Categories;
using Common.Models.Products;
using Microsoft.Extensions.Options;

namespace ApiGateway.Services.Catalog
{
    public class CatalogService : ICatalogService
    {
        private readonly IConsumer<Category> _categoryConsumer;
        private readonly IConsumer<Product> _productConsumer;
        private readonly UrlsConfig _urls;

        public CatalogService(IConsumer<Category> categoryConsumer, IConsumer<Product> productConsumer, IOptions<UrlsConfig> config)
        {
            _categoryConsumer = categoryConsumer;
            _productConsumer = productConsumer;
            _urls = config.Value;
        }
    }
}
