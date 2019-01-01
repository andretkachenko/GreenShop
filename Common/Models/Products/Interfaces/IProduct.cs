using Common.Models.Categories.Interfaces;
using Common.Models.Comments.Interfaces;
using Common.Models.Entity.Interfaces;
using System.Collections.Generic;

namespace Common.Models.Products.Interfaces
{
    public interface IProduct : IEntity
    {
        string Description { get; set; }

        decimal BasePrice { get; set; }
        float Rating { get; set; }
        int CategoryId { get; set; }

        ICategory Category { get; set; }
        IEnumerable<IComment> Comments { get; set; }
    }
}
