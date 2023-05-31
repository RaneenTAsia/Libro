using Application.DTOs;
using Domain.Enums;
using FluentValidation;

namespace Presentation.Validators
{
    public class RoleToUpdateDTOValidator : AbstractValidator<RoleToUpdateDTO>
    {
        public RoleToUpdateDTOValidator()
        { 
            RuleFor(u => (Role) u.Role)
            .IsInEnum().WithMessage("Must be within set roles");
        }
    }
}
