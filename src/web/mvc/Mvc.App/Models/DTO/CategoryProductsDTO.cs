using GreenShop.Web.Mvc.App.Models.Categories;
using GreenShop.Web.Mvc.App.Models.Products;
using System.Collections.Generic;

namespace GreenShop.Web.Mvc.App.Models.DTO
{
    public class CategoryProductsDTO
    {
        public Category Category { get; set; }
        public List<Product> Products { get; set; }
    }
}
