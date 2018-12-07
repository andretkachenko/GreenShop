using Common.Models.Categories.Interfaces;

namespace Common.Models.Categories
{
    public class Category : ICategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public bool IsSubCategory { get; set; }

        public ICategory SubCategory { get; set; }
    }
}
