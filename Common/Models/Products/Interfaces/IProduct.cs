using Common.Models.Categories.Interfaces;
using Common.Models.Specifications;
using System.Collections.Generic;

namespace Common.Models.Products.Interfaces
{
    public interface IProduct
    {
        int Id { get; set; }
        string Name { get; set; }

        string Description { get; set; }

        decimal BasePrice { get; set; }
        float Rating { get; set; }

        ICategory Category { get; set; }
        List<ISpecification> Specifications { get; set; }
    }
}
