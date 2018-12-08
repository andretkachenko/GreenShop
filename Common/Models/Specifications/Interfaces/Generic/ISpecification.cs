namespace Common.Models.Specifications.Interfaces.Generic
{
    public interface ISpecification<T> : ISpecification
    {
        T Value { get; set; }
    }
}
