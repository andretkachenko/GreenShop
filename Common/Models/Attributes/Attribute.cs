using Common.Models.Attributes.Interfaces.Generic;
using System.Collections.Generic;

namespace Common.Models.Attributes
{
    public class Attribute<T> : IAttribute<T>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int MaxSelectionAvailable { get; set; }
        public IEnumerable<T> Options { get; set; }
    }
}
