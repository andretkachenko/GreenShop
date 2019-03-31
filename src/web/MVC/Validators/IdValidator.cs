using FluentValidation;

namespace GreenShop.MVC.Validators
{
    public class IdValidator : AbstractValidator<int>
    {
        public IdValidator()
        {
            RuleFor(id => id).GreaterThan(0);
        }
    }
}
