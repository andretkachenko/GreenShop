using GreenShop.Catalog.Domain.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.Wrappers
{
    internal class CommentWrapper : Comment
    {
        public CommentWrapper(int id, int authorId, string message, int productId) : base(authorId, message, productId)
        {
            Id = id;
        }
    }
}
