using FluentValidation;

namespace GreenShop.Catalog.Validators
{
    public class IdValidator : AbstractValidator<int>
    {
        public IdValidator()
        {
            RuleFor(id => id).NotEqual(default(int));
        }
    }
}
