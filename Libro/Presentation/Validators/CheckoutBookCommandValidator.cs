using Application.Entities.Books.Commands;
using FluentValidation;

namespace Presentation.Validators
{
    public class CheckoutBookCommandValidator : AbstractValidator<CheckoutBookCommand>
    {
        public CheckoutBookCommandValidator()
        {
            RuleFor(cb => cb.UserId)
                .NotEmpty().WithMessage("Cannot leave UserId empty")
                .Must(c => c.GetType() == typeof(int))
                .GreaterThanOrEqualTo(1).WithMessage("Must be valid userId");

            RuleFor(cb => cb.BookId)
                .NotEmpty().WithMessage("Cannot leave BookId empty")
                .Must(c => c.GetType() == typeof(Int32)).WithMessage("Must be a number")
                .GreaterThanOrEqualTo(1).WithMessage("Must be valid bookId");

            RuleFor(cb => cb.DueDate)
                .Must(c => c.GetType() == typeof(DateTime))
                .GreaterThan(DateTime.UtcNow)
                .When(cb => cb.DueDate.HasValue)
                .WithMessage("The date must be after today's date");
        }
    }
}
