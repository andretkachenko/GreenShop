using FluentValidation;

namespace GreenShop.Web.Mvc.App.Validators
{
    public class EntityNameValidator : AbstractValidator<string>
    {
        public EntityNameValidator()
        {
            RuleFor(name => name).NotEmpty();
        }
    }
}
