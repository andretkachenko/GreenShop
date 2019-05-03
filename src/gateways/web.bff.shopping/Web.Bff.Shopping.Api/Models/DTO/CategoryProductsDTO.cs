using GreenShop.Web.Bff.Shopping.Api.Models.Categories;
using GreenShop.Web.Bff.Shopping.Api.Models.Products;
using System.Collections.Generic;

namespace GreenShop.Web.Bff.Shopping.Api.Models.DTO
{
    public class CategoryProductsDTO
    {
        public Category Category { get; set; }
        public List<Product> Products { get; set; }
    }
}
