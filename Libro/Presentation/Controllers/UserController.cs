using Application.DTOs;
using Application.Users.Commands;
using Application.Users.Queries;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> RegisterUserAsync([FromBody] CreateUserCommand userForCreation)
        {
            if (userForCreation == null)
                return NotFound();

            if(!ModelState.IsValid || !TryValidateModel(userForCreation)) 
                return BadRequest(ModelState);

            var result = await _mediator.Send(userForCreation);

            if(result.Item2 == Result.Failed)
                return BadRequest("Email already exists");

            return Ok(result.Item1);
        }
    }
}
