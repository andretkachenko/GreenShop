namespace Common.Models.Categories.Interfaces
{
    public interface ICategory
    {
        int Id { get; set; }
        string Name { get; set; }

        bool IsSubCategory { get; set; }

        ICategory SubCategory { get; set; }
    }
}
