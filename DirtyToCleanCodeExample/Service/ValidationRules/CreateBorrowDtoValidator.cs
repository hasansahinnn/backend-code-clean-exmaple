using FluentValidation;
using Service.Constants;
using Service.DTOs;

namespace Service.ValidationRules;

/// <summary>
/// Validator for the CreateBorrowDto.
/// </summary>
public class CreateBorrowDtoValidator : AbstractValidator<CreateBorrowDto>
{
    public CreateBorrowDtoValidator()
    {
        RuleFor(x => x.MemberId)
            .NotNull().WithMessage(Messages.IdCannotBeNull)
            .GreaterThan(0).WithMessage(Messages.IdMustBeGreaterThanZero);
    }
}