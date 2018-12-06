using Common.Models.Categories.Interfaces;
using Common.Models.Comments;
using Common.Models.Entity.Interfaces;
using Common.Models.Specifications;
using System.Collections.Generic;

namespace Common.Models.Products.Interfaces
{
    public interface IProduct : IEntity
    {
        string Description { get; set; }

        decimal BasePrice { get; set; }
        float Rating { get; set; }

        ICategory Category { get; set; }
        List<ISpecification> Specifications { get; set; }
        List<IComment> Comments { get; set; }
    }
}
