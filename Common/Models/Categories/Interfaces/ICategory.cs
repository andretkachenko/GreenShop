using Common.Models.Entity.Interfaces;

namespace Common.Models.Categories.Interfaces
{
    public interface ICategory : IEntity
    {
        bool IsSubCategory { get; set; }

        ICategory SubCategory { get; set; }
    }
}
