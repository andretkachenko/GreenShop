using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace GreenShop.Catalog.Domain.Products
{
    public class Specification : IValueObject
    {
        [BsonElement("name")]
        public string Name { get; protected set; }
        [BsonElement("max_selection_available")]
        public int MaxSelectionAvailable { get; protected set; }
        [BsonElement("options")]
        public IEnumerable<string> Options { get; protected set; }

        public Specification(string name, int maxSelect, IEnumerable<string> options)
        {
            Name = name;
            MaxSelectionAvailable = maxSelect;
            Options = options;
        }
    }
}
