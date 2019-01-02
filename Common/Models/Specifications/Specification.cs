using Common.Models.Specifications.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Common.Models.Specifications
{
    public class Specification : ISpecification
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("max_selection_available")]
        public int MaxSelectionAvailable { get; set; }
        [BsonElement("options")]
        public IEnumerable<string> Options { get; set; }
    }
}
