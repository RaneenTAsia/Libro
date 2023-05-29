using Application.Users.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Validators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(u => u.Email)
                .EmailAddress().WithMessage("Must be in the form of an email address")
                .NotEmpty().WithMessage("Cannot leave Email empty");

            RuleFor(u => u.Username)
                .MinimumLength(5).WithMessage("Username must be longer than 5 letters")
                .MaximumLength(30).WithMessage("Username must be shorter than 30 characters")
                .NotEmpty().WithMessage("Username must not be empty");

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
