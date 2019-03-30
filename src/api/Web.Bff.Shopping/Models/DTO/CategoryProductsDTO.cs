using GreenShop.Web.Bff.Shopping.Models.Categories;
using GreenShop.Web.Bff.Shopping.Models.Products;
using System.Collections.Generic;

namespace GreenShop.Web.Bff.Shopping.Models.DTO
{
    public class CategoryProductsDTO
    {
        public Category Category { get; set; }
        public List<Product> Products { get; set; }
    }
}
