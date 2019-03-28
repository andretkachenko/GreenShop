namespace GreenShop.Catalog.Models.Comments
{
    public class Comment : IComment
    {
        public int Id { get; set; }

        public int AuthorId { get; set; }

        public string Message { get; set; }

        public int ProductId { get; set; }
    }
}
