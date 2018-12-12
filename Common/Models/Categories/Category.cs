using Common.Models.Categories.Interfaces;
using System;

namespace Common.Models.Categories
{
    public class Category : ICategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int ParentCategoryId { get; set; }

        public ICategory SubCategory { get; set; }

        public override bool Equals(object obj)
        {
            // If parameter is null or cannot be cast to Category return false.
            if (obj == null || !(obj is Category that))
            {
                return false;
            }

            // Return true if the fields match:
            return (Id == that.Id) 
                && (Name == that.Name) 
                && (ParentCategoryId == that.ParentCategoryId) 
                && (SubCategory == that.SubCategory);
        }

        public bool Equals(Category obj)
        {
            // If parameter is null return false:
            if (obj == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (Id == obj.Id)
                && (Name == obj.Name)
                && (ParentCategoryId == obj.ParentCategoryId)
                && (SubCategory == obj.SubCategory);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, ParentCategoryId, SubCategory);
        }
    }
}
