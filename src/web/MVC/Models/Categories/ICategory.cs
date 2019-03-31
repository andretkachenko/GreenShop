namespace GreenShop.MVC.Models.Categories
{
    public interface ICategory : IEntity, IIdentifiable
    {
        int ParentCategoryId { get; set; }
    }
}
