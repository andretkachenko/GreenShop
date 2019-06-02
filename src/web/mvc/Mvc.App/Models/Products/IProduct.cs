using GreenShop.Web.Mvc.App.Models.Categories;
using GreenShop.Web.Mvc.App.Models.Comments;
using GreenShop.Web.Mvc.App.Models.Specifications;
using System.Collections.Generic;

namespace GreenShop.Web.Mvc.App.Models.Products
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
