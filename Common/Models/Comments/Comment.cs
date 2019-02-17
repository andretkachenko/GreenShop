using Common.Models.Comments.Interfaces;
using Common.Models.Products.Interfaces;

namespace Common.Models.Comments
{
    public class Comment : IComment
    {
        public int Id { get; set; }

        public int AuthorId { get; set; }

        public string Message { get; set; }

        public int ProductId { get; set; }
    }
}
