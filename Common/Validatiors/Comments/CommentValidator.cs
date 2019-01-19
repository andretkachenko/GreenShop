using Common.Models.Comments;
using FluentValidation;

namespace Common.Validatiors.Comments
{
    public class CommentValidator : AbstractValidator<Comment>
    {
        public CommentValidator()
        {
            RuleFor(comment => comment.Id).GreaterThan(0);
            RuleFor(comment => comment.Message).NotNull();
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
