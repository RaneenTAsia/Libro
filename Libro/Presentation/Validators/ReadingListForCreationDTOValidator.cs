using Application.DTOs;
using FluentValidation;

namespace Presentation.Validators
{
    public class ReadingListForCreationDTOValidator : AbstractValidator<ReadingListForCreationDTO>
    {
        public ReadingListForCreationDTOValidator()
        {
            RuleFor(e => e.Title)
                .MaximumLength(255).WithMessage("Title is too long")
                .NotEmpty().WithMessage("Must have a title");

        }
    }
}
