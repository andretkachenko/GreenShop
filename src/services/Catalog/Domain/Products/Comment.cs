namespace GreenShop.Catalog.Domain.Products
{
    public class Comment : IEntity
    {
        #region Constructors
        /// <summary>
        /// Controller used by the Dapper in order to map obtain from DB
        /// values into thr Enitty model.
        /// Apart from this use-case, it should never be called.
        /// </summary>
        protected Comment() { }

        public Comment(int authorId, string message, int productId)
        {
            AuthorId = authorId;
            Message = message;
            ProductId = productId;
        }
        #endregion

        #region Properties
        public int Id { get; protected set; }
        public int AuthorId { get; protected set; }
        public string Message { get; protected set; }
        public int ProductId { get; protected set; }
        #endregion

        #region Setters
        /// <summary>
        /// Edit message of the Comment
        /// </summary>
        /// <param name="Message">New variant of the message</param>
        /// <returns>Result flag</returns>
        public void UpdateMessage(string newMessage)
        {
            Message = newMessage;
        }
        #endregion
    }
}
