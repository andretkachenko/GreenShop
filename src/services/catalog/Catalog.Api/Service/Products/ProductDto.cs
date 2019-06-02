using System.Collections.Generic;

namespace GreenShop.Catalog.Api.Service.Products
{
    public class ProductDto
    {
        public int Id { get; set; }

        public char StatusCode { get; set; }
        public string MongoId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public float Rating { get; set; }
        public int CategoryId { get; set; }
        public IEnumerable<CommentDto> Comments { get; set; }
        public IEnumerable<SpecificationDto> Specifications { get; set; }
    }
}
