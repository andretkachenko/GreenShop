namespace GreenShop.Catalog.Models.Categories
{
    public interface ICategory : IEntity, IIdentifiable
    {
        int ParentCategoryId { get; set; }

        ICategory SubCategory { get; set; }
    }
}
