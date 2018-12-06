using Common.Models.Entity.Interfaces;

namespace Common.Models.Comments
{
    public interface IComment : IIdentifiable
    {

        string Author { get; set; }

        string Message { get; set; }
    }
}
