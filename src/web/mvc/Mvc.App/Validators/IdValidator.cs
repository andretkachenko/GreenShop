using FluentValidation;

namespace GreenShop.Web.Mvc.App.Validators
{
    public class IdValidator : AbstractValidator<int>
    {
        public IdValidator()
        {
            RuleFor(id => id).GreaterThan(0);
        }
    }
}
