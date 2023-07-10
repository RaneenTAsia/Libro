using Application.Entities.Users.Commands;
using Application.Entities.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationsController : ControllerBase
    {
        public readonly IMediator _mediator;

        public AuthenticationsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUserAsync([FromBody] CreateUserCommand userForCreation)
        {
            if (userForCreation == null)
                return NotFound();

            if (!ModelState.IsValid || !TryValidateModel(userForCreation))
                return BadRequest(ModelState);

            var result = await _mediator.Send(userForCreation);

            return result;
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync([FromBody] AuthenticateUserQuery userToLogin)
        {
            if (userToLogin == null)
                return NotFound();

            if (!ModelState.IsValid || !TryValidateModel(userToLogin))
                return BadRequest(ModelState);

            var result = await _mediator.Send(userToLogin);

            return result;
        }
    }
}
