namespace GreenShop.Web.Mvc.App.Models.Comments
{
    public interface IComment : IIdentifiable
    {
        int ProductId { get; set; }
        int AuthorId { get; set; }
        string Message { get; set; }
    }
}
