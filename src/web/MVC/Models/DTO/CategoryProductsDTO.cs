using GreenShop.MVC.Models.Categories;
using GreenShop.MVC.Models.Products;
using System.Collections.Generic;

namespace GreenShop.MVC.Models.DTO
{
    public class CategoryProductsDTO
    {
        public Category Category { get; set; }
        public List<Product> Products { get; set; }
    }
}
