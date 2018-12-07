using System.Collections.Generic;

namespace Common.Models.Attributes.Interfaces.Generic
{
    public interface IAttribute<T> : IAttribute
    {
        int MaxSelectionAvailable { get; set; }
        IEnumerable<T> Options { get; set; }
    }
}
