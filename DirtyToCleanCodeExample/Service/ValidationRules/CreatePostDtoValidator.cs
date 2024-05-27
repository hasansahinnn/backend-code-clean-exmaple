using FluentValidation;
using Service.DTOs;
using static Service.Constants.Messages;

namespace Service.ValidationRules;

/// <summary>
/// Validator for the CreatePostDto.
/// </summary>
public class CreatePostDtoValidator : AbstractValidator<CreatePostDto>
{
    public CreatePostDtoValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull().WithMessage(IdCannotBeNull)
            .GreaterThan(0).WithMessage(IdMustBeGreaterThanZero);

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage(ContentCannotBeEmpty)
            .NotNull().WithMessage(ContentCannotBeNull);
    }
}