using Common.Models.Categories.Interfaces;
using Common.Models.Comments.Interfaces;
using Common.Models.Products.Interfaces;
using Common.Models.Specifications.Interfaces;
using System.Collections.Generic;

namespace Common.Models.Products
{
    public class Product : IProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal BasePrice { get; set; }
        public float Rating { get; set; }

        public ICategory Category { get; set; }
        public IEnumerable<ISpecification> Specifications { get; set; }
        public IEnumerable<IComment> Comments { get; set; }
    }
}
