using FluentValidation;
using GreenShop.Catalog.Api.Service.Products;

namespace GreenShop.Catalog.Api.Validators
{
    public class CommentValidator : AbstractValidator<CommentDto>
    {
        public CommentValidator()
        {
            RuleFor(comment => comment.ProductId).NotNull();
            RuleFor(comment => comment.AuthorId).NotNull();
            RuleFor(comment => comment.ProductId).NotEqual(default(int));
            RuleFor(comment => comment.AuthorId).NotEqual(default(int));
            RuleFor(comment => comment.Message).NotNull();
        }
    }
    public class CommentMessageValidator : AbstractValidator<string>
    {
        public CommentMessageValidator()
        {
            RuleFor(message => message).NotEmpty();
        }
    }
}
