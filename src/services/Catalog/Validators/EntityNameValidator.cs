using FluentValidation;

namespace GreenShop.Catalog.Validators
{
    public class EntityNameValidator : AbstractValidator<string>
    {
        public EntityNameValidator()
        {
            RuleFor(name => name).NotEmpty();
        }
    }
}
