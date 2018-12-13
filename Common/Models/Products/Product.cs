using Common.Models.Categories.Interfaces;
using Common.Models.Comments.Interfaces;
using Common.Models.Products.Interfaces;
using Common.Models.Specifications.Interfaces;
using Common.Validatiors;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;

namespace Common.Models.Products
{
    public class Product : IProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal BasePrice { get; set; }
        public float Rating { get; set; }

        public int CategoryId { get; set; }
        
        [Write(false)]
        public ICategory Category { get; set; }
        [Write(false)]
        public IEnumerable<ISpecification> Specifications { get; set; }
        [Write(false)]
        public IEnumerable<IComment> Comments { get; set; }

        public override bool Equals(object obj)
        {
            // If parameter is null or cannot be cast to Category return false.
            if (obj == null || !(obj is Product that))
            {
                return false;
            }

            // Return true if the fields match:
            return EqualityValidator.ReflectiveEquals(this, that);
        }

        public bool Equals(Product obj)
        {
            // If parameter is null return false:
            if (obj == null)
            {
                return false;
            }

            // Return true if the fields match:
            return EqualityValidator.ReflectiveEquals(this, obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, CategoryId);
        }
    }
}
