using System;

namespace GreenShop.Catalog.Infrastructure.Categories
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ParentCategoryId { get; set; }
    }
}
