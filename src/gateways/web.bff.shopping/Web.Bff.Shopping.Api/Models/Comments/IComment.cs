namespace GreenShop.Web.Bff.Shopping.Api.Models.Comments
{
    public interface IComment : IIdentifiable
    {
        int ProductId { get; set; }
        int AuthorId { get; set; }
        string Message { get; set; }
    }
}
