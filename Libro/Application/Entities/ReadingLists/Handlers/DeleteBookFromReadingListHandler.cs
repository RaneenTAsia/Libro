using Application.Entities.ReadingLists.Commands;
using Domain.Enums;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Application.Entities.ReadingLists.Handlers
{
    public class DeleteBookFromReadingListHandler : IRequestHandler<DeleteBookFromReadingListCommand, ActionResult>
    {
        public readonly IBookRepository _bookRepository;
        public readonly IReadingListsRepository _readingListsRepository;
        public readonly IReadingItemsRepository _readingItemsRepository;
        public readonly ILogger<DeleteBookFromReadingListHandler> _logger;

        public DeleteBookFromReadingListHandler(IBookRepository bookRepository, IReadingListsRepository readingListsRepository, IReadingItemsRepository readingItemsRepository, ILogger<DeleteBookFromReadingListHandler> logger)
        {
            _bookRepository = bookRepository;
            _readingListsRepository = readingListsRepository;
            _readingItemsRepository = readingItemsRepository;
            _logger = logger;
        }

        public async Task<ActionResult> Handle(DeleteBookFromReadingListCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Check if Book {0} exists", request.BookId);
            if (!(await _bookRepository.BookExistsAsync(request.BookId)))
            {
                _logger.LogDebug("Book {0} does not exist", request.BookId);
                return new NotFoundObjectResult("Book does not exist");
            }

            _logger.LogDebug("Check if ReadingList {0} exists", request.ReadingListId);
            if (!(await _readingListsRepository.ReadingListExistsAsync(request.ReadingListId)))
            {
                _logger.LogDebug("ReadingList {0} does not exist", request.ReadingListId);
                return new NotFoundObjectResult("ReadingList  does not exist");
            }

            _logger.LogDebug("Check if Book {0} exists in ReadingList {1}", request.BookId, request.ReadingListId);
            if (!(await _readingItemsRepository.BookExistsInListAsync(request.BookId, request.ReadingListId)))
            {
                _logger.LogDebug("Book {0} does not exist in ReadingList {1}", request.BookId, request.ReadingListId);
            }

            _logger.LogDebug(" Deleting Book {0} from ReadingList {1}", request.BookId, request.ReadingListId);
            var result = await _readingItemsRepository.DeleteBookFromReadingListAsync(request.BookId, request.ReadingListId);

            if (result == Result.Failed)
            {
                return new BadRequestObjectResult("Book was not deleted from reading list");
            }

            return new OkObjectResult("Successfully deleted book from reading list");
        }
    }
}
