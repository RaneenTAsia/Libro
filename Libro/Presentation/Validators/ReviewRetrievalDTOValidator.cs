using Application.DTOs;
using FluentValidation;

namespace Presentation.Validators
{
    public class ReviewRetrievalDTOValidator : AbstractValidator<ReviewRetrievalDTO>
    {
        public ReviewRetrievalDTOValidator()
        {
            RuleFor(r => r.Rating)
                .IsInEnum().WithMessage("Rating must be within the appropriate range")
                .NotNull().WithMessage("Cannot keep rating empty");

            RuleFor(r => r.ReviewContent)
                .MinimumLength(30).WithMessage("Must be longer")
                .MaximumLength(255).WithMessage("Review Too long")
                .When(r => r.ReviewContent != null);
        }
    }
}
