using Dapper.Contrib.Extensions;
using GreenShop.Catalog.Domain;
using System;

namespace GreenShop.Catalog.Domain.Categories
{
    [Table("Categories")]
    public class Category : IAggregate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ParentCategoryId { get; set; }
        [Write(false)]
        public Category SubCategory { get; set; }
    }
}
