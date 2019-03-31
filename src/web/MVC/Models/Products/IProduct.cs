using GreenShop.MVC.Models.Categories;
using GreenShop.MVC.Models.Comments;
using GreenShop.MVC.Models.Specifications;
using System.Collections.Generic;

namespace GreenShop.MVC.Models.Products
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
