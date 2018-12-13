using Common.Models.Categories;
using FluentValidation;

namespace Common.Validatiors.Categories
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(category => category.Id).GreaterThan(0);
            RuleFor(category => category.Name).NotNull();
        }
    }

    public class CategoryIdValidator : AbstractValidator<int>
    {
        public CategoryIdValidator()
        {
            RuleFor(id => id).GreaterThan(0);
        }
    }

    public class CategoryNameValidator : AbstractValidator<string>
    {
        public CategoryNameValidator()
        {
            RuleFor(name => name).NotEmpty();
        }
    }
}
