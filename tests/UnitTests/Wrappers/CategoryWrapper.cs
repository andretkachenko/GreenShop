using GreenShop.Catalog.Domain.Categories;

namespace UnitTests.Wrappers
{
    internal class CategoryWrapper : Category
    {
        public int WrapId { set => Id = value; }
        public string WrapName { set => Name = value; }
        public int WrapParentCategoryId { set => ParentCategoryId = value; }
    }
}
