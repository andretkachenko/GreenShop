namespace GreenShop.Catalog.Api.Service.Categories
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public char StatusCode { get; set; }
        public int ParentCategoryId { get; set; }
    }
}
