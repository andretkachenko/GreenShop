using System;

namespace GreenShop.Catalog.Models.Comments
{
    public class Comment
    {
        public Guid AuthorId { get; set; }
        public string Message { get; set; }
        public Guid ProductId { get; set; }
    }
}
