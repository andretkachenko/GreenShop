using System.Collections.Generic;

namespace GreenShop.Catalog.Models.Specifications
{
    public interface ISpecification : IEntity
    {
        int MaxSelectionAvailable { get; set; }
        IEnumerable<string> Options { get; set; }
    }
}
