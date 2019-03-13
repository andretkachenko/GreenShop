using Common.Models.Categories;
using Common.Models.Products;
using System.Collections.Generic;

namespace Common.Models.DTO
{
    public class CategoryProductsDTO
    {
        public Category Category { get; set; }
        public List<Product> Products { get; set; }
    }
}
