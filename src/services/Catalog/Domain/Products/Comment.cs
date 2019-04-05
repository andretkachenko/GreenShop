using GreenShop.Catalog.Domain;
using System;

namespace GreenShop.Catalog.Models.Comments
{
    public class Comment : IEntity
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }
        public string Message { get; set; }
        public Guid ProductId { get; set; }
    }
}
