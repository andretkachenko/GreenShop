using GreenShop.Catalog.Models.Categories;
using GreenShop.Catalog.Models.Comments;
using GreenShop.Catalog.Models.Specifications;
using System.Collections.Generic;

namespace GreenShop.Catalog.Models.Products
{
    public interface IProduct : IEntity, IIdentifiable
    {
        string MongoId { get; }
        string Description { get; }

        decimal BasePrice { get; }
        float Rating { get; }
        int CategoryId { get; }

        Category Category { get; set; }
        IEnumerable<IComment> Comments { get; set; }
        IEnumerable<Specification> Specifications { get; set; }
    }
}
