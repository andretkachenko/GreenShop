using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace GreenShop.Catalog.Api.Domain.Products
{
    public class Specification : IValueObject
    {
        #region Constructors
        /// <summary>
        /// Controller used by the Dapper in order to map obtain from DB
        /// values into thr Enitty model.
        /// Apart from this use-case, it should never be called.
        /// </summary>
        private Specification() { }

        public Specification(string name, int maxSelect, IEnumerable<string> options)
        {
            Name = name;
            MaxSelectionAvailable = maxSelect;
            Options = options;
        }
        #endregion

        #region Properties
        [BsonElement("name")]
        public string Name { get; protected set; }
        [BsonElement("max_selection_available")]
        public int MaxSelectionAvailable { get; protected set; }
        [BsonElement("options")]
        public IEnumerable<string> Options { get; protected set; }
        #endregion
    }
}
