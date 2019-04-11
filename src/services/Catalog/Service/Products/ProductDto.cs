using System;
using System.Collections.Generic;

namespace GreenShop.Catalog.Service.Products
{
    public class ProductDto
    {
        public Guid Id { get; protected set; }
        public string MongoId { get; protected set; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public decimal BasePrice { get; protected set; }
        public float Rating { get; protected set; }
        public Guid CategoryId { get; protected set; }
        public IEnumerable<CommentDto> Comments { get; set; }
        public IEnumerable<SpecificationDto> Specifications { get; set; }
    }
}
