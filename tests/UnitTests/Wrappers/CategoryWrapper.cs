using GreenShop.Catalog.Domain.Categories;

namespace UnitTests.Wrappers
{
    internal class CategoryWrapper : Category
    {
        public CategoryWrapper(int id, string name, int parentId) : base(name, parentId)
        {
            Id = id;
        }
    }
}
