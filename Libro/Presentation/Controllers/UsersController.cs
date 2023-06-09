﻿using Application.DTOs;
using Application.Entities.Users.Commands;
using Application.Entities.Users.Queries;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

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

            return result;
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

            return result;
        }

        [Authorize]
        [HttpGet("{userId}/history")]
        public async Task<ActionResult> GetUserBorrowingHistory(int userId, int pageNumber = 1, int pageSize = 10)
        {
            var tokenUserId = Convert.ToInt32(User.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"));
            var tokenUserRole = User.FindFirstValue("http://schemas.microsoft.com/ws/2008/06/identity/claims/role");

            if (tokenUserId != userId && tokenUserRole.Equals(Role.Patron.ToString()))
                return Unauthorized();

            var request = new GetUserHistoryQuery { UserId = userId, pageNumber = pageNumber, pageSize = pageSize };

            var result = await _mediator.Send(request);

            Response.Headers.Add("X-Pagination",
               JsonSerializer.Serialize(result.Item2));

            return result.Item1;
        }

        [Authorize]
        [HttpPut("{userId}/update")]
        public async Task<ActionResult> UpdateUserEmail(int userId, ProfileToUpdateDTO profile)
        {
            if (!ModelState.IsValid || !TryValidateModel(profile))
            {
                return BadRequest(ModelState);
            }

            var tokenUserId = Convert.ToInt32(User.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"));
            var tokenUserRole = User.FindFirstValue("http://schemas.microsoft.com/ws/2008/06/identity/claims/role");

            var request = new UpdateUserProfileCommand { UserId = userId, Profile = profile, TokenUserId = tokenUserId, TokenUserRole = tokenUserRole };

            var result = await _mediator.Send(request);

            return result;
        }

        [Authorize(Policy = "MustBeLibrarian")]
        [HttpPost("mail")]
        public async Task<ActionResult> SendPatronEmailAsync([FromForm] SendEmailToUserQuery request)
        {
            if (!ModelState.IsValid || !TryValidateModel(request))
            {
                return BadRequest(ModelState);
            }

            var result = await _mediator.Send(request);

            return result;
        }
    }
}
