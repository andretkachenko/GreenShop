using Common.Models.Specifications.Interfaces.Generic;

namespace Common.Models.Specifications
{
    public class Specification<T> : ISpecification<T>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public T Value { get; set; }
    }
}
