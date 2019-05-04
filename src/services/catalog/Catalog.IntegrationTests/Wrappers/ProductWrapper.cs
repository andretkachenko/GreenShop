using GreenShop.Catalog.Api.Domain.Products;
using System.Collections.Generic;

namespace GreenShop.Catalog.IntegrationTests.Wrappers
{
    internal class ProductWrapper : Product
    {
        public int WrapId { set => Id = value; }
        public string WrapMongoId { set => MongoId = value; }
        public string WrapName { set => Name = value; }
        public string WrapDescription { set => Description = value; }
        public decimal WrapBasePrice { set => BasePrice = value; }
        public float WrapRating { set => Rating = value; }
        public int WrapCategoryId { set => CategoryId = value; }
        public List<Specification> WrapSpecifications { set => Specifications = value; }
        public List<Comment> WrapComments { set => Comments = value; }
    }
}
