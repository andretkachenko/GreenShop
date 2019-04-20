using Dapper.Contrib.Extensions;

namespace GreenShop.Catalog.Domain.Categories
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
        private Category() { }

        public Category(string name)
        {
            Name = name;
        }

        public Category(string name, int parentId)
        {
            Name = name;
            ParentCategoryId = parentId;
        }
        #endregion

        #region Properties
        public int Id { get; protected set; }
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
