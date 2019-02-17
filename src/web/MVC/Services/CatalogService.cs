using Microsoft.Extensions.Options;
using MvcWebApp.Config;
using MvcWebApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcWebApp.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly UrlsConfig _urls;

        public CatalogService(IOptions<UrlsConfig> config)
        {
            _urls = config.Value;
        }
    }
}
