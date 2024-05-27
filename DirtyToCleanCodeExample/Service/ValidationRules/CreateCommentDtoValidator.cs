using FluentValidation;
using Service.Constants;
using Service.DTOs;

namespace Service.ValidationRules;

/// <summary>
/// Validator for the CreateCommentDto.
/// </summary>
public class CreateCommentDtoValidator : AbstractValidator<CreateCommentDto>
{
    public CreateCommentDtoValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull().WithMessage(Messages.IdCannotBeNull)
            .GreaterThan(0).WithMessage(Messages.IdMustBeGreaterThanZero);

        RuleFor(x => x.PostId)
            .NotNull().WithMessage(Messages.IdCannotBeNull)
            .GreaterThan(0).WithMessage(Messages.IdMustBeGreaterThanZero);

        RuleFor(x => x.Text)
            .NotNull().WithMessage(Messages.CommentTextCannotBeNull)
            .NotEmpty().WithMessage(Messages.CommentTextCannotBeEmpty);
    }
}