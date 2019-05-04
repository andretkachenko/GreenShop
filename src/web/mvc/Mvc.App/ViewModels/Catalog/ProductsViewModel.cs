using GreenShop.Web.Mvc.App.Models.Products;
using System.Collections.Generic;

namespace GreenShop.Web.Mvc.App.ViewModels.Catalog
{
    public class ProductsViewModel
    {
        public IEnumerable<Product> Products { get; set; }
    }
}
