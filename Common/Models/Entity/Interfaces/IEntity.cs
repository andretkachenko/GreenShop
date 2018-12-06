namespace Common.Models.Entity.Interfaces
{
    public interface IEntity : IIdentifiable
    {
        string Name { get; set; }
    }
}
