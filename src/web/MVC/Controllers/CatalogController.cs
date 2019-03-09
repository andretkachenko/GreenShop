using Common.Models.Categories;
using Common.Models.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MvcWebApp.Config;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Linq;

namespace MVC.Controllers
{
    public class CatalogController : Controller
    {
        private readonly UrlsConfig _urls;
        private readonly IRestClient _restClient;

        public CatalogController(IRestClient restClient,
                                 IOptionsSnapshot<UrlsConfig> config)
        {
            _urls = config.Value;
            _restClient = restClient;
            _restClient.BaseUrl = new System.Uri(_urls.WebShoppingApi);
        }

        public IActionResult Index()
        {
            RestRequest request = new RestRequest(UrlsConfig.WebShoppingApiOperations.CategoryApiOperations.GetAllCategories, Method.GET);
            request.AddHeader("Cache-Control", "no-cache");
            IRestResponse response = _restClient.Execute(request);
            List<Category> categories = JsonConvert.DeserializeObject<List<Category>>(response.Content);

            ViewData["categories"] = categories;

            return View();
        }

        public IActionResult Category(int id)
        {
            RestRequest request = new RestRequest(UrlsConfig.WebShoppingApiOperations.CategoryApiOperations.GetCategory(id), Method.GET);
            request.AddHeader("Cache-Control", "no-cache");
            IRestResponse response = _restClient.Execute(request);
            Category Category = JsonConvert.DeserializeObject<Category>(response.Content);

            request = new RestRequest(UrlsConfig.WebShoppingApiOperations.ProductApiOperations.GetAllProducts, Method.GET);
            response = _restClient.Execute(request);
            List<Product> products = JsonConvert.DeserializeObject<List<Product>>(response.Content);

            products = products?.Where(p => p.CategoryId == id).ToList() ?? new List<Product>();

            ViewData["category"] = Category;
            ViewData["products"] = products;

            return View();
        }

        public IActionResult AllProducts()
        {
            RestRequest request = new RestRequest(UrlsConfig.WebShoppingApiOperations.ProductApiOperations.GetAllProducts, Method.GET);
            request.AddHeader("Cache-Control", "no-cache");
            IRestResponse response = _restClient.Execute(request);
            List<Product> products = JsonConvert.DeserializeObject<List<Product>>(response.Content);

            ViewData["products"] = products;

            return View("Products");
        }

        public IActionResult Product(int id)
        {
            RestRequest request = new RestRequest(UrlsConfig.WebShoppingApiOperations.ProductApiOperations.GetProduct(id), Method.GET);
            request.AddHeader("Cache-Control", "no-cache");
            IRestResponse response = _restClient.Execute(request);
            Product product = JsonConvert.DeserializeObject<Product>(response.Content);

            RestRequest request2 = new RestRequest(UrlsConfig.WebShoppingApiOperations.CategoryApiOperations.GetCategory(product.CategoryId), Method.GET);
            request2.AddHeader("Cache-Control", "no-cache");
            IRestResponse response2 = _restClient.Execute(request);
            Category Category = JsonConvert.DeserializeObject<Category>(response2.Content);

            ViewData["product"] = product;
            ViewData["category"] = Category;

            return View();
        }
    }
}
