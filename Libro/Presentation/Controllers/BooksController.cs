using Application.DTOs;
using Application.Entities.Books.Commands;
using Application.Entities.Books.Queries;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace Presentation.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        public readonly IMediator _mediator;

        public BooksController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("search")]
        public async Task<ActionResult> SearchBooksAsync(string? title, string? author, int? genre, int pageNumber = 1, int pageSize = 10)
        {

            var query = new SearchBooksQuery { Title = title, Author = author, GenreId = genre, pageNumber = pageNumber, pageSize = pageSize };

            var result = await _mediator.Send(query);

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(result.Item2));

            return Ok(result.Item1);
        }

        [HttpGet("browse")]
        public async Task<ActionResult> BrowseBooksAsync(int pageNumber = 1, int pageSize = 10)
        {
            var query = new BrowseAvailableBooksQuery { pageNumber = pageNumber, pageSize = pageSize };

            var result = await _mediator.Send(query);

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(result.Item2));

            return Ok(result.Item1);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetBookIdAsync(int id)
        {
            var query = new GetBookDetailsQuery { BookId = id };

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost("{id}/reserve")]
        [Authorize(Policy = "MustBePatron")]
        public async Task<ActionResult> ReserveBookAsync(int id)
        {
            var userId = Convert.ToInt32(User.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"));
            var command = new ReserveBookCommand { UserId = userId, BookId = id };
            var result = await _mediator.Send(command);

            if (result.Item1 == Domain.Enums.Result.Failed)
            {
                return BadRequest(result.Item2);
            }

            return Ok(result.Item2);
        }

        [HttpPost("checkout")]
        [Authorize(Policy = "MustBeLibrarian")]
        public async Task<ActionResult> CheckoutBookAsync(CheckoutBookCommand command)
        {
            if (command == null)
                return NotFound();

            if (!ModelState.IsValid || !TryValidateModel(command))
                return BadRequest(ModelState);

            var result = await _mediator.Send(command);

            return result;
        }

        [HttpPost("return")]
        [Authorize(Policy = "MustBeLibrarian")]
        public async Task<ActionResult> ReturnBookAsync(ReturnBookCommand command)
        {
            if (command == null)
                return NotFound();

            if (!ModelState.IsValid || !TryValidateModel(command))
                return BadRequest(ModelState);

            var result = await _mediator.Send(command);

            return result;
        }

        [HttpGet("overdue")]
        [Authorize(Policy = "MustBeLibrarian")]
        public async Task<ActionResult> GetOverdueBookAsync(int pageNumber = 1, int pageSize = 10)
        {
            var request = new TrackOverdueBooksQuery { pageNumber = pageNumber, pageSize = pageSize };

            var result = await _mediator.Send(request);

            if (result.Item1.Count < 0)
            {
                return BadRequest();
            }
            else if (result.Item1.Count == 0)
            {
                return Ok("There are no Overdue Books");
            }

            Response.Headers.Add("X-Pagination",
               JsonSerializer.Serialize(result.Item2));

            return Ok(result.Item1);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator,Librarian")]
        public async Task<ActionResult> CreateBook(AddBookCommand command)
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

        [HttpPut("{bookId}")]
        [Authorize(Roles = "Administrator,Librarian")]
        public async Task<ActionResult> UpdateBook(int bookId, BookRetrievalDTO bookDTO)
        {
            if (bookDTO == null)
                return BadRequest();
            var command = new UpdateBookCommand() { BookId = bookId, RetrievedBookDTO = bookDTO };

            if (command == null)
                return NotFound();

            if (!ModelState.IsValid || !TryValidateModel(command))
                return BadRequest(ModelState);

            var result = await _mediator.Send(command);

            return result;
        }

        [HttpDelete("{bookId}")]
        [Authorize(Roles = "Administrator,Librarian")]
        public async Task<ActionResult> DeleteBook(int bookId)
        {
            var command = new DeleteBookCommand() { BookId = bookId };

            if (command == null)
                return NotFound();

            var result = await _mediator.Send(command);

            return result;
        }

        [HttpPost("{bookId}/favorite")]
        [Authorize(Policy = "MustBePatron")]
        public async Task<ActionResult> SaveBookAsync(int bookId, int readingList)
        {
            var command = new SaveBookCommand { BookId = bookId, ReadingListId = 1 };
            var result = await _mediator.Send(command);

            if (result.Item1 == Result.Failed)
            {
                return BadRequest(result.Item2);
            }

            return Ok(result.Item2);
        }
    }
}
