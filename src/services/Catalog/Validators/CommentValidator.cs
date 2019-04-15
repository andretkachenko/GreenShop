using FluentValidation;
using GreenShop.Catalog.Service.Products;
using System;

namespace GreenShop.Catalog.Validators
{
    public class CommentValidator : AbstractValidator<CommentDto>
    {
        public CommentValidator()
        {
            RuleFor(comment => comment.ProductId).NotNull();
            RuleFor(comment => comment.AuthorId).NotNull();
            RuleFor(comment => comment.ProductId).NotEqual(Guid.Empty);
            RuleFor(comment => comment.AuthorId).NotEqual(Guid.Empty);
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
