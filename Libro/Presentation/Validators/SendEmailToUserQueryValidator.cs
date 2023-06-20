using Application.Entities.Users.Queries;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace Presentation.Validators
{
    public class SendEmailToUserQueryValidator : AbstractValidator<SendEmailToUserQuery>
    {
        public SendEmailToUserQueryValidator()
        {
            RuleFor(e => e.ToEmail)
                .EmailAddress().WithMessage("Must be in the form of an email address")
                .NotEmpty().WithMessage("Cannot leave Reciever Email empty");

            RuleFor(e => e.Subject)
                .MaximumLength(30)
                .NotEmpty().WithMessage("Must have Subject");

            RuleFor(e => e.Body)
                .NotEmpty().WithMessage("Must have content to send");
        }
    }
}
