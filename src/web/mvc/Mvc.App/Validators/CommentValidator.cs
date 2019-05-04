using FluentValidation;
using GreenShop.Web.Mvc.App.Models.Comments;

namespace GreenShop.Web.Mvc.App.Validators
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
