using Application.DTOs;
using Application.Entities.Users.Commands;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [Authorize(Policy = "MustBeAdministrator")]
        [HttpPut("{userId}")]
        public async Task<ActionResult> ChangeRole(int userId, [FromBody] RoleToUpdateDTO role)
        {
            if (role == null)
                return BadRequest();

            if (!ModelState.IsValid || !TryValidateModel(role))
                return BadRequest(ModelState);

            var request = new ChangeUserRoleCommand { Role = role.Role, UserId = userId };

            var result = await _mediator.Send(request);

            if (result.Item1 == Result.Failed)
            {
                return BadRequest(result.Item2);
            }

            return Ok(result.Item2);
        }
    }
}
