using Application.Entities.Books.Commands;
using FluentValidation;

namespace Presentation.Validators
{
    public class ReturnBookCommandValidator : AbstractValidator<ReturnBookCommand>
    {
        public ReturnBookCommandValidator()
        {
            RuleFor(cb => cb.BookId)
                .NotEmpty().WithMessage("Cannot leave BookId empty")
                .Must(c => c.GetType() == typeof(Int32)).WithMessage("Must be a number")
                .GreaterThanOrEqualTo(1).WithMessage("Must be valid bookId");
        }
    }
}
