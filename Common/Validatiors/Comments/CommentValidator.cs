using Common.Models.Comments;
using FluentValidation;

namespace Common.Validatiors.Comments
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
    public class CommenMessageValidator : AbstractValidator<string>
    {
        public CommenMessageValidator()
        {
            RuleFor(message => message).NotEmpty();
        }
    }
    public class CommentIdValidator : AbstractValidator<int>
    {
        public CommentIdValidator()
        {
            RuleFor(id => id).GreaterThan(0);
        }
    }
}
