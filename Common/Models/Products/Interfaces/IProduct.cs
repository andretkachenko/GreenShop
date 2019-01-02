using Common.Models.Categories.Interfaces;
using Common.Models.Comments.Interfaces;
using Common.Models.Entity.Interfaces;
using Common.Models.Specifications;
using System.Collections.Generic;

namespace Common.Models.Products.Interfaces
{
    public interface IProduct : IEntity, IIdentifiable
    {
        string MongoId { get; set; }
        string Description { get; set; }

        decimal BasePrice { get; set; }
        float Rating { get; set; }
        int CategoryId { get; set; }

        ICategory Category { get; set; }
        IEnumerable<IComment> Comments { get; set; }
        IEnumerable<Specification> Specifications { get; set; }
    }
}
