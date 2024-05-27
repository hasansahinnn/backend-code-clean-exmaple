using Data.Models;
using FluentValidation;
using Service.Constants;
using Service.DTOs;

namespace Service.ValidationRules;

/// <summary>
/// Validator for the PublishPostDto.
/// </summary>
public class PublishPostDtoValidator : AbstractValidator<PublishPostDto>
{
    public PublishPostDtoValidator()
    {
        RuleFor(x => x.PostId)
            .NotNull().WithMessage(Messages.IdCannotBeNull)
            .GreaterThan(0).WithMessage(Messages.IdMustBeGreaterThanZero);

        RuleFor(x => x.Status)
            .Must(BeAValidStatus).WithMessage(Messages.InvalidPostStatusValue);
    }

    private bool BeAValidStatus(int status)
    {
        return Enum.IsDefined(typeof(PostStatus), status);
    }
}