﻿using FluentValidation;
using GreenShop.Catalog.Models.Comments;

namespace GreenShop.Catalog.Validators
{
    public class CommentValidator : AbstractValidator<Comment>
    {
        public CommentValidator()
        {
            RuleFor(comment => comment.ProductId).GreaterThan(0);
            RuleFor(comment => comment.AuthorId).GreaterThan(0);
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