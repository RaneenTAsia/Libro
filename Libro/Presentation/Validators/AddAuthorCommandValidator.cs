using Application.Entities.Authors.Commands;
using FluentValidation;

namespace Presentation.Validators
{
    public class AddAuthorCommandValidator : AbstractValidator<AddAuthorCommand>
    {
        public AddAuthorCommandValidator()
        {
            RuleFor(e => e.Name)
                    .MaximumLength(30).WithMessage("Name is too long")
                    .NotEmpty().WithMessage("Must have a Name");

            RuleFor(e => e.Description)
                    .MinimumLength(30).WithMessage("Description too short")
                    .When(e => e.Description != null);

            RuleFor(e => e.BookIds)
                    .Must(e => e.Count > 0).WithMessage("There Must be at least one BookId")
                    .When(e => e.BookIds != null);
        }
    }
}
