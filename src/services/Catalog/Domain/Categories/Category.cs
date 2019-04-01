using Dapper.Contrib.Extensions;

namespace GreenShop.Catalog.Models.Categories
{
    [Table("Categories")]
    public class Category : ICategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int ParentCategoryId { get; set; }
        
        [Write(false)]
        public ICategory SubCategory { get; set; }
    }
}
