namespace GreenShop.Catalog.Service.Products
{
    public class CommentDto
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string Message { get; set; }
        public int ProductId { get; set; }
    }
}
