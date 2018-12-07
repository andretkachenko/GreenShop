using Common.Models.Entity.Interfaces;

namespace Common.Models.Comments.Interfaces
{
    public interface IComment : IIdentifiable
    {
        string Author { get; set; }

        string Message { get; set; }
    }
}
