using System.Collections.Generic;

namespace GreenShop.Web.Mvc.App.Models.Specifications
{
    public interface ISpecification : IEntity
    {
        int MaxSelectionAvailable { get; set; }
        IEnumerable<string> Options { get; set; }
    }
}
