namespace GreenShop.Catalog.Domain.Products
{
    public class Comment : IEntity
    {
        public Comment(int authorId, string message, int productId)
        {
            AuthorId = authorId;
            Message = message;
            ProductId = productId;
        }

        public int Id { get; protected set; }
        public int AuthorId { get; protected set; }
        public string Message { get; protected set; }
        public int ProductId { get; protected set; }

        /// <summary>
        /// Edit message of the Comment
        /// </summary>
        /// <param name="Message">New variant of the message</param>
        /// <returns>Result flag</returns>
        public void UpdateMessage(string newMessage)
        {
            Message = newMessage;
        }
    }
}
