using Application.Entities.Books.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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

            var request = await _mediator.Send(query);

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(request.Item2));

            return Ok(request.Item1);
        }

        [HttpGet("browse")]
        public async Task<ActionResult> BrowseBooksAsync( int pageNumber = 1, int pageSize = 10)
        {
            var query = new BrowseAvailableBooksQuery {  pageNumber = pageNumber, pageSize = pageSize };

            var request = await _mediator.Send(query);

            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(request.Item2));

            return Ok(request.Item1);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetBookId(int id)
        {
            var query = new GetBookDetailsQuery { BookId= id };

            var request = await _mediator.Send(query);

            return Ok(request);
        }
    }
}
