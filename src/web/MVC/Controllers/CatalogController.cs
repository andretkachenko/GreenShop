using GreenShop.MVC.Models.Categories;
using GreenShop.MVC.Models.Comments;
using GreenShop.MVC.Models.DTO;
using GreenShop.MVC.Models.Products;
using GreenShop.MVC.Services.Interfaces;
using GreenShop.MVC.ViewModels.Catalog;
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

            IndexViewModel model = new IndexViewModel
            {
                Categories = categories
            };

            return View(model);
        }

        public async Task<IActionResult> Category(int id)
        {
            CategoryProductsDTO categoryWithProducts = await _catalogService.GetCategoryWithProductsAsync(id);
            if (categoryWithProducts.Category == null) return NotFound();

            CategoryViewModel model = new CategoryViewModel
            {
                Category = categoryWithProducts.Category,
                Products = categoryWithProducts.Products
            };

            return View(model);
        }

        public async Task<IActionResult> AllProducts()
        {
            IEnumerable<Product> products = await _catalogService.GetAllProductsAsync();
            if (products == null) return NotFound();

            ProductsViewModel model = new ProductsViewModel
            {
                Products = products
            };

            return View("Products", model);
        }

        public async Task<IActionResult> Product(int id)
        {
            Product product = await _catalogService.GetProductWithCategoryAsync(id);
            if (product == null) return NotFound();
            IEnumerable<Comment> comment = await _catalogService.GetAllProductComments(id);
            product.Comments = comment;
            ProductViewModel model = new ProductViewModel
            {
                Product = product
            };

            return View(model);
        }
    }
}
