using Application.Entities.Users.Queries;
using FluentValidation;

namespace Presentation.Validators
{
    public class AuthenticateUserQueryValidator :AbstractValidator<AuthenticateUserQuery>
    {
        public AuthenticateUserQueryValidator()
        {
            RuleFor(u => u.Email)
                .EmailAddress().WithMessage("Must be in the form of an email address")
                .NotEmpty().WithMessage("Cannot leave Email empty");

            RuleFor(u => u.Password)
                .MinimumLength(5).WithMessage("Password must be longer than 5 characters")
                .MaximumLength(30).WithMessage("Password must be shorter than 30 characters")
                .NotEmpty().WithMessage("Cannot leave password empty")
                .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.");
        }
    }
}
