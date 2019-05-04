using GreenShop.Web.Mvc.App.Models.Categories;
using GreenShop.Web.Mvc.App.Models.Comments;
using GreenShop.Web.Mvc.App.Models.Specifications;
using System.Collections.Generic;

namespace GreenShop.Web.Mvc.App.Models.Products
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
