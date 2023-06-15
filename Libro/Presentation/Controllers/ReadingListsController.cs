using Application.DTOs;
using Application.Entities.Books.Commands;
using Application.Entities.ReadingLists.Commands;
using Application.Entities.ReadingLists.Queries;
using Application.Entities.Users.Queries;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;

namespace Presentation.Controllers
{
    [Route("api/users/{userId}/readinglists")]
    [ApiController]
    public class ReadingListsController : ControllerBase
    {
        public readonly IMediator _mediator;

        public ReadingListsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [Authorize(Policy = "MustBePatron")]
        [HttpGet]
        public async Task<ActionResult> GetReadingLists(int userId , int pageNumber =1, int pageSize = 10)
        {
            var query = new GetUserReadingListsQuery { UserId = userId,  pageNumber = pageNumber, pageSize = pageSize};

            var result = await _mediator.Send(query);

            Response.Headers.Add("X-Pagination",
               JsonSerializer.Serialize(result.Item2));

            return result.Item1;
        }

        [HttpDelete]
        [Authorize(Policy = "MustBePatron")]
        public async Task<ActionResult> DeleteBookFromListAsync(int userId, DeleteBookFromReadingListCommand command)
        {
            var tokenUserId = Convert.ToInt32(User.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"));
            if(tokenUserId != userId)
            {
                return Unauthorized();
            }

            var result = await _mediator.Send(command);

            return result;
        }
    }
}
