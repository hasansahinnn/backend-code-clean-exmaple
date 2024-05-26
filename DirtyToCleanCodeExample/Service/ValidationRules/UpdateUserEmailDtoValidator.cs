using FluentValidation;
using Service.Constants;
using Service.DTOs;

namespace Service.ValidationRules;

/// <summary>
/// Validator for the UpdateUserEmailDto.
/// </summary>
public class UpdateUserEmailDtoValidator : AbstractValidator<UpdateUserEmailDto>
{
    public UpdateUserEmailDtoValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull().WithMessage(Messages.IdCannotBeNull)
            .GreaterThan(0).WithMessage(Messages.IdMustBeGreaterThanZero);

        RuleFor(x => x.Email)
            .NotNull().WithMessage(Messages.UserEmailCannotBeNull)
            .EmailAddress().WithMessage(Messages.YouMustEnterAValidEmail);
    }
}