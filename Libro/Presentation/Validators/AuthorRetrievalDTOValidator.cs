using Application.DTOs;
using FluentValidation;

namespace Presentation.Validators
{
    public class AuthorRetrievalDTOValidator : AbstractValidator<AuthorRetrievalDTO>
    {
        public AuthorRetrievalDTOValidator()
        {
            RuleFor(e => e.Name)
                   .MaximumLength(255).WithMessage("Title is too long")
                   .NotEmpty().WithMessage("Must have a title");

            RuleFor(e => e.Description)
                    .MinimumLength(30).WithMessage("Description too short")
                    .When(e => e.Description != null);

            RuleFor(e => e.BookIds)
                     .Must(e => e.Count > 0).WithMessage("There Must be at least one BookId")
                     .When(e => e.BookIds != null);
        }
    }
}
