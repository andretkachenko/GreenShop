using Dapper.Contrib.Extensions;
using System;

namespace GreenShop.Catalog.Domain.Categories
{
    [Table("Categories")]
    public class Category : IAggregate
    {
        public int Id { get; protected set; }
        public string Name { get; protected set; }
        public int ParentCategoryId { get; protected set; }
        [Write(false)]
        public Category SubCategory { get; protected set; }

        public Category(string name)
        {
            Name = name;
        }

        public Category(string name, int parentId)
        {
            Name = name;
            ParentCategoryId = parentId;
        }

        /// <summary>
        /// Update Category Name 
        /// </summary>
        /// <param name="newName">New Name for the Category</param>
        public void ChangeCategoryName(string newName)
        {
            Name = newName;
        }

        /// <summary>
        /// Move Category to the different parent Category
        /// </summary>
        /// <param name="id">Guid of the Category, that should become parent</param>
        public void ChangeParentCategory(int id)
        {
            ParentCategoryId = id;
        }
    }
}
