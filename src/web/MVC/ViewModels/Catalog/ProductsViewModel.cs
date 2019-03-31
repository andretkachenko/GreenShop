using GreenShop.MVC.Models.Products;
using System.Collections.Generic;

namespace GreenShop.MVC.ViewModels.Catalog
{
    public class ProductsViewModel
    {
        public IEnumerable<Product> Products { get; set; }
    }
}
