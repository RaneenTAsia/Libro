using Application.DTOs;
using Application.Entities.Users.Commands;
using Application.Entities.Users.Queries;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [Authorize]
        [HttpGet("{userId}")]
        public async Task<ActionResult> GetUserProfile(int userId)
        {
            var tokenUserId = Convert.ToInt32(User.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"));
            var tokenUserRole = User.FindFirstValue("http://schemas.microsoft.com/ws/2008/06/identity/claims/role");

            if (tokenUserId != userId && tokenUserRole.Equals(Role.Patron.ToString()))
                return Unauthorized();

            var request = new GetUserQuery { UserId = userId };

            var result = await _mediator.Send(request);

            if (result.Item1 == null)
            {
                return BadRequest(result.Item2);
            }

            return Ok(result.Item1);
        }

        [Authorize]
        [HttpGet("{userId}/history")]
        public async Task<ActionResult> GetUserBorrowingHistory(int userId, int pageNumber = 1, int pageSize = 10)
        {
            var tokenUserId = Convert.ToInt32(User.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"));
            var tokenUserRole =User.FindFirstValue("http://schemas.microsoft.com/ws/2008/06/identity/claims/role");

            if (tokenUserId != userId && tokenUserRole.Equals(Role.Patron.ToString()))
                return Unauthorized();

            var request = new GetUserHistoryQuery { UserId = userId, pageNumber = pageNumber, pageSize = pageSize };

            var result = await _mediator.Send(request);

            if (result.Item1 == null)
            {
                return BadRequest(result.Item2);
            }

            return Ok(result.Item1);
        }
    }
}
