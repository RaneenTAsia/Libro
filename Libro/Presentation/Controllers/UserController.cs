﻿using Application.DTOs;
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

        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginAsync([FromBody] AuthenticateUserQuery userToLogin)
        {
            if(userToLogin == null)
                return NotFound();

            var result = await _mediator.Send(userToLogin);

            if(result.Item2 == Result.Failed)
                return Unauthorized(result.Item1);

            return Ok(result.Item1);
        }
    }
}
