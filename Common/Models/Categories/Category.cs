using Common.Models.Categories.Interfaces;
using Common.Validatiors;
using Dapper.Contrib.Extensions;
using System;

namespace Common.Models.Categories
{
    [Table("Categories")]
    public class Category : ICategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int ParentCategoryId { get; set; }
        
        [Write(false)]
        public ICategory SubCategory { get; set; }

        public override bool Equals(object obj)
        {
            // If parameter is null or cannot be cast to Category return false.
            if (obj == null || !(obj is Category that))
            {
                return false;
            }

            // Return true if the fields match:
            return EqualityValidator.ReflectiveEquals(this, that);
        }

        public bool Equals(Category obj)
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
            return HashCode.Combine(Id, Name, ParentCategoryId, SubCategory);
        }
    }
}
