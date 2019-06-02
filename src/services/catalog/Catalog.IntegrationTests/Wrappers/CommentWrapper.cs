using GreenShop.Catalog.Api.Domain.Products;

namespace GreenShop.Catalog.IntegrationTests.Wrappers
{
    internal class CommentWrapper : Comment
    {
        public int WrapId { set => Id = value; }
        public int WrapAuthorId { set => AuthorId = value; }
        public string WrapMessage { set => Message = value; }
        public int WrapProductId { set => ProductId = value; }
    }
}
