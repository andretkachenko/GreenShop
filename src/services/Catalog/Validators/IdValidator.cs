using FluentValidation;
using System;

namespace GreenShop.Catalog.Validators
{
    public class IdValidator : AbstractValidator<Guid>
    {
        public IdValidator()
        {
            RuleFor(id => id).NotNull();
            RuleFor(id => id).NotEqual(Guid.Empty);
        }
    }
}
