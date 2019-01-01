using Common.Models.Attributes.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Common.Models.Attributes
{
    public class Specification : ISpecification
    {
        [BsonId]
        public int Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("max_selection_available")]
        public int MaxSelectionAvailable { get; set; }
        [BsonElement("options")]
        public IEnumerable<string> Options { get; set; }
    }
}
