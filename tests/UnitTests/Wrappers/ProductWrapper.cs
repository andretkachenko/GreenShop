using GreenShop.Catalog.Domain.Products;
using System.Collections.Generic;

namespace UnitTests.Wrappers
{
    internal class ProductWrapper : Product
    {
        public ProductWrapper(int id, string mongoId, decimal basePrice, float rating, string name, int categoryId, string description) 
            : base(name, categoryId, description)
        {
            Id = id;
            MongoId = mongoId;
            BasePrice = basePrice;
            Rating = rating;
        }

        public ProductWrapper(string mongoId, IEnumerable<Specification> specifications, string name, int categoryId, string description)
            : base(name, categoryId, description)
        {
            MongoId = mongoId;
            Specifications = specifications;
        }
    }
}
