using GreenShop.Catalog.Models.Categories;
using GreenShop.Catalog.Models.Comments;
using GreenShop.Catalog.Models.Specifications;
using System.Collections.Generic;

namespace GreenShop.Catalog.Models.Products
{
    public interface IProduct : IEntity, IIdentifiable
    {
        string MongoId { get; set; }
        string Description { get; set; }

        decimal BasePrice { get; set; }
        float Rating { get; set; }
        int CategoryId { get; set; }

        Category Category { get; set; }
        IEnumerable<IComment> Comments { get; set; }
        IEnumerable<Specification> Specifications { get; set; }
    }
}
