using Application.DTOs;
using Application.Entities.Books.Commands;
using Application.Entities.Reviews.Commands;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [Route("api/books/{bookId}/reviews")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        public readonly IMediator _mediator;

        public ReviewsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        [Authorize(Policy = "MustBePatron")]
        public async Task<ActionResult> ReviewBookAsync(int bookId, ReviewRetrievalDTO createReviewDTO)
        {
            var tokenUserId = Convert.ToInt32(User.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"));

            if (createReviewDTO == null)
                return NotFound();

            if (!ModelState.IsValid || !TryValidateModel(createReviewDTO))
                return BadRequest(ModelState);

            var command = new CreateReviewCommand { BookId = bookId, UserId = tokenUserId, CreateReviewDTO = createReviewDTO };
            var result = await _mediator.Send(command);

            return result;
        }

        [HttpPut]
        [Authorize(Policy = "MustBePatron")]
        public async Task<ActionResult> UpdateReviewAsync(int bookId, ReviewRetrievalDTO updateReviewDTO)
        {
            var tokenUserId = Convert.ToInt32(User.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"));

            if (updateReviewDTO == null)
                return NotFound();

            if (!ModelState.IsValid || !TryValidateModel(updateReviewDTO))
                return BadRequest(ModelState);

            var command = new UpdateReviewCommand { BookId = bookId, UserId = tokenUserId, CreateReviewDTO = updateReviewDTO };
            var result = await _mediator.Send(command);

            return result;
        }

        [HttpDelete]
        [Authorize(Policy = "MustBePatron")]
        public async Task<ActionResult> DeleteReviewAsync(int bookId)
        {
            var tokenUserId = Convert.ToInt32(User.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"));

            var command = new DeleteReviewCommand { BookId = bookId, UserId = tokenUserId};

            var result = await _mediator.Send(command);

            return result;
        }
    }
}
