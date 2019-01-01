using Common.Models.Entity.Interfaces;
using System.Collections.Generic;

namespace Common.Models.Attributes.Interfaces
{
    public interface ISpecification : IEntity
    {
        int MaxSelectionAvailable { get; set; }
        IEnumerable<string> Options { get; set; }
    }
}
