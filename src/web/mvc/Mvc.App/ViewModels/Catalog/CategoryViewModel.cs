using GreenShop.Web.Mvc.App.Models.Categories;
using GreenShop.Web.Mvc.App.Models.Products;
using System.Collections.Generic;

namespace GreenShop.Web.Mvc.App.ViewModels.Catalog
{
    public class CategoryViewModel
    {
        public Category Category { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
