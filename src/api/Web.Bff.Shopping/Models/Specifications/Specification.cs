using System.Collections.Generic;

namespace GreenShop.Web.Bff.Shopping.Models.Specifications
{
    public class Specification : ISpecification
    {
        public string Name { get; set; }
        public int MaxSelectionAvailable { get; set; }
        public IEnumerable<string> Options { get; set; }
    }
}
