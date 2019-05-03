using GreenShop.Web.Bff.Shopping.Api.Models.Categories;
using GreenShop.Web.Bff.Shopping.Api.Models.Comments;
using GreenShop.Web.Bff.Shopping.Api.Models.Specifications;
using System.Collections.Generic;

namespace GreenShop.Web.Bff.Shopping.Api.Models.Products
{
    public class Product : IProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public float Rating { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public IEnumerable<IComment> Comments { get; set; }
        public IEnumerable<Specification> Specifications { get; set; }
    }
}
