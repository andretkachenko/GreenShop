using FluentValidation;

namespace Common.Validatiors
{
    public class IdValidator : AbstractValidator<int>
    {
        public IdValidator()
        {
            RuleFor(id => id).GreaterThan(0);
        }
    }
}
