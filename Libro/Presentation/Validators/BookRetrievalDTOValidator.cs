using Application.DTOs;
using FluentValidation;

namespace Presentation.Validators
{
    public class BookRetrievalDTOValidator : AbstractValidator<BookRetrievalDTO>
    {
        public BookRetrievalDTOValidator()
        {
            RuleFor(e => e.Title)
                    .MaximumLength(255).WithMessage("Title is too long")
                    .NotEmpty().WithMessage("Must have a title");

            RuleFor(e => e.Description)
                    .MinimumLength(30).WithMessage("Description too short")
                    .When(e => e.Description != null);

            RuleFor(e => e.PageAmount)
                    .GreaterThanOrEqualTo(1).WithMessage("There are not enough pages")
                    .NotEmpty().WithMessage("There must be a specified page amount");

            RuleFor(e => e.PublishDate)
                .NotEmpty().WithMessage("There must be a publish date")
                .Must(e => e < DateTime.UtcNow).WithMessage("Publish date cannot be for later than today");

            RuleFor(e => e.BookAuthors)
                .NotEmpty().WithMessage("there must be a bookAuthors list")
                .Must(e => e.Count > 0).WithMessage("Must have at least 1 author");

            RuleFor(e => e.Genres)
                .NotEmpty().WithMessage("there must be a genres list")
                .Must(e => e.Count > 0).WithMessage("Must have at least 1 genre");
                
        }
    }
}
