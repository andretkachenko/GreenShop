using FluentValidation;

namespace Common.Validatiors
{
    public class EntityNameValidator : AbstractValidator<string>
    {
        public EntityNameValidator()
        {
            RuleFor(name => name).NotEmpty();
        }
    }
}
