using FluentValidation;

namespace GreenShop.Catalog.Api.Validators
{
    public class EntityNameValidator : AbstractValidator<string>
    {
        public EntityNameValidator()
        {
            RuleFor(name => name).NotEmpty();
        }
    }
}
