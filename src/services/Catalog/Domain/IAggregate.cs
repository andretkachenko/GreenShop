using System;

namespace GreenShop.Catalog.Models
{
    public interface IAggregate
    {
        Guid Id { get; }
    }
}
