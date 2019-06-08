using Dapper.Contrib.Extensions;
using System;

namespace GreenShop.Catalog.Api.Domain.Categories
{
    [Table("Categories")]
    public class Category : IAggregate
    {
        #region Constructors
        /// <summary>
        /// Controller used by the Dapper in order to map obtain from DB
        /// values into thr Enitty model.
        /// Apart from this use-case, it should never be called.
        /// </summary>
        protected Category() { }

        public Category(string name)
        {
            Name = name;
            StatusCode = CategoryStatus.Active;
        }

        public Category(string name, int parentId)
        {
            Name = name;
            ParentCategoryId = parentId;
            StatusCode = CategoryStatus.Active;
        }
        #endregion

        #region Properties
        public int Id { get; protected set; }
        public char StatusCode { get; protected set; }
        public string Name { get; protected set; }
        public int ParentCategoryId { get; protected set; }
        [Write(false)]
        public Category SubCategory { get; protected set; }
        #endregion

        #region Setters
        /// <summary>
        /// Update Category Name 
        /// </summary>
        /// <param name="newName">New Name for the Category</param>
        public void ChangeCategoryName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName)) throw new ArgumentNullException("New Name");

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
        #endregion
    }
}
