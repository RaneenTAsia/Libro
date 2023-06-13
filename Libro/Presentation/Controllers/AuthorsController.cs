using Application.Entities.Authors.Commands;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorsController :ControllerBase
    {
        public readonly IMediator _mediator;

        public AuthorsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [Authorize(Roles ="Administrator,Librarian")]
        public async Task<ActionResult> CreateAuthor(AddAuthorCommand command)
        {
            if (command == null)
                return NotFound();

            if (!ModelState.IsValid || !TryValidateModel(command))
                return BadRequest(ModelState);

            var result = await _mediator.Send(command);

            if (result.Item1 == Result.Failed)
                return BadRequest(result.Item2);

            return Ok(result.Item2);
        }
    }
}
