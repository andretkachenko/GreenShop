using Common.Models.Entity.Interfaces;

namespace Common.Models.Comments.Interfaces
{
    public interface IComment : IIdentifiable
    {
        int AuthorId { get; set; }

        string Message { get; set; }
    }
}
