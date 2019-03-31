using FluentValidation;

namespace GreenShop.MVC.Validators
{
    public class EntityNameValidator : AbstractValidator<string>
    {
        public EntityNameValidator()
        {
            RuleFor(name => name).NotEmpty();
        }
    }
}
