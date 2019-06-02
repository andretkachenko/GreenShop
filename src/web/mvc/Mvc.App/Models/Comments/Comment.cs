namespace GreenShop.Web.Mvc.App.Models.Comments
{
    public class Comment : IComment
    {
        public int Id { get; set; }

        public int AuthorId { get; set; }

        public string Message { get; set; }

        public int ProductId { get; set; }
    }
}
