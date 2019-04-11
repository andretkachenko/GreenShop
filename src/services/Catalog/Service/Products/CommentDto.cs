using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenShop.Catalog.Infrastructure.Products
{
    public class CommentDto
    {
        public Guid AuthorId { get; set; }
        public string Message { get; set; }
        public Guid ProductId { get; set; }
    }
}
