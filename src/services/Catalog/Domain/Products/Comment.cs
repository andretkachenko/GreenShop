using GreenShop.Catalog.Domain;
using System;

namespace GreenShop.Catalog.Models.Comments
{
    public class Comment : IEntity
    {
        public Comment(Guid authorId, string message, Guid productId)
        {
            Id = new Guid();
            AuthorId = authorId;
            Message = message;
            ProductId = productId;
        }

        public Guid Id { get; protected set; }
        public Guid AuthorId { get; protected set; }
        public string Message { get; protected set; }
        public Guid ProductId { get; protected set; }

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
