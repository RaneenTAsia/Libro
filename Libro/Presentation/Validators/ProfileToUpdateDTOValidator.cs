using Application.DTOs;
using FluentValidation;

namespace Presentation.Validators
{
    public class ProfileToUpdateDTOValidator : AbstractValidator<ProfileToUpdateDTO>
    {
        public ProfileToUpdateDTOValidator()
        {
            RuleFor(u => u.Email)
                .EmailAddress().WithMessage("Must be in the form of an email address")
                .NotEmpty().WithMessage("Cannot leave Email empty");

            RuleFor(u => u.Username)
                .MinimumLength(5).WithMessage("Username must be longer than 5 letters")
                .MaximumLength(30).WithMessage("Username must be shorter than 30 characters")
                .NotEmpty().WithMessage("Username must not be empty");
        }
    }
}
