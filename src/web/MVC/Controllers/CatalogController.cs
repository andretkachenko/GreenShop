using Common.Models.Categories;
using Common.Models.DTO;
using Common.Models.Products;
using GreenShop.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenShop.MVC.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;

        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Category> categories = await _catalogService.GetAllCategoriesAsync();
            if (categories == null) return NotFound();

            ViewData["categories"] = categories;

            return View();
        }

        public async Task<IActionResult> Category(int id)
        {
            CategoryProductsDTO categoryWithProducts = await _catalogService.GetCategoryWithProductsAsync(id);
            if (categoryWithProducts.Category == null) return NotFound();

            ViewData["category"] = categoryWithProducts.Category;
            ViewData["products"] = categoryWithProducts.Products;

            return View();
        }

        public async Task<IActionResult> AllProducts()
        {
            IEnumerable<Product> products = await _catalogService.GetAllProductsAsync();
            if (products == null) return NotFound();

            ViewData["products"] = products;

            return View("Products");
        }

        public async Task<IActionResult> Product(int id)
        {
            Product product = await _catalogService.GetProductWithCategoryAsync(id);

            if (product == null) return NotFound();

            ViewData["product"] = product;

            return View();
        }
    }
}
