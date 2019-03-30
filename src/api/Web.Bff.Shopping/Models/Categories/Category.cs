namespace GreenShop.Web.Bff.Shopping.Models.Categories
{
    public class Category : ICategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int ParentCategoryId { get; set; }

        public ICategory SubCategory { get; set; }
    }
}
