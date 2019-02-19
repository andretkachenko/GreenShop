using Common.Models.Entity.Interfaces;

namespace Common.Models.Categories.Interfaces
{
    public interface ICategory : IEntity, IIdentifiable
    {
        int ParentCategoryId { get; set; }

        ICategory SubCategory { get; set; }
    }
}
