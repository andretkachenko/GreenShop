using Common.Models.Comments;
using FluentValidation;

namespace Common.Validatiors.Comments
{
    public class CommentValidator : AbstractValidator<Comment>
    {
        public CommentValidator()
        {
            RuleFor(comment => comment.ProductId).GreaterThan(0);
            RuleFor(comment => comment.Author).NotNull();
            RuleFor(comment => comment.Id).GreaterThan(0);
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
    public class CommentIdValidator : AbstractValidator<int>
    {
        public CommentIdValidator()
        {
            RuleFor(id => id).GreaterThan(0);
        }
    }
}
