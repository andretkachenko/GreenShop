namespace GreenShop.Web.Mvc.App.Models.Categories
{
    public interface ICategory : IEntity, IIdentifiable
    {
        int ParentCategoryId { get; set; }
    }
}
