using Common.Models.Categories;
using Common.Models.Products;
using System.Collections.Generic;

namespace GreenShop.MVC.ViewModels.Catalog
{
    public class CategoryViewModel
    {
        public Category Category { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
