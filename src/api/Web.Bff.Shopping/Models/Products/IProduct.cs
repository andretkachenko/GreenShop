using GreenShop.Web.Bff.Shopping.Models.Categories;
using GreenShop.Web.Bff.Shopping.Models.Comments;
using GreenShop.Web.Bff.Shopping.Models.Specifications;
using System.Collections.Generic;

namespace GreenShop.Web.Bff.Shopping.Models.Products
{
    public interface IProduct : IEntity, IIdentifiable
    {
        string Description { get; set; }

        decimal BasePrice { get; set; }
        float Rating { get; set; }
        int CategoryId { get; set; }

        Category Category { get; set; }
        IEnumerable<IComment> Comments { get; set; }
        IEnumerable<Specification> Specifications { get; set; }
    }
}
