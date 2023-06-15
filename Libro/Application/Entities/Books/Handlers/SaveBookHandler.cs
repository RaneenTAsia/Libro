using Application.Entities.Books.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Books.Handlers
{
    public class SaveBookHandler : IRequestHandler<SaveBookCommand, (Result, string)>
    {
        public readonly IBookRepository _bookRepository;
        public readonly IReadingItemsRepository _readingItemsRepository;
        public readonly ILogger<SaveBookHandler> _logger;

        public SaveBookHandler(IBookRepository bookRepository, IReadingItemsRepository readingItemsRepository, ILogger<SaveBookHandler> logger)
        {
            _bookRepository = bookRepository;
            _readingItemsRepository = readingItemsRepository;
            _logger = logger;
        }

        public async Task<(Result, string)> Handle(SaveBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Checking if Book {0} exists", request.BookId);

            if (!(await _bookRepository.BookExistsAsync(request.BookId)))
            {
                return (Result.Failed, "Book Does Not Exist");
            }

            _logger.LogDebug("Checking if Book {0} exists in Reading List {1}", request.BookId, request.ReadingListId);
            if (await _readingItemsRepository.BookExistsInListAsync(request.BookId, request.ReadingListId))
            {
                return (Result.Failed, "Book already in List");
            }

            _logger.LogDebug("Adding Book {0} to Reading List {1}", request.BookId, request.ReadingListId);
            var result = await _readingItemsRepository.AddBookToReadingList(request.BookId, request.ReadingListId);

            if (result == Result.Failed)
            {
                _logger.LogDebug("Failed to add Book {0} to Reading List {1}", request.BookId, request.ReadingListId);
                return (Result.Failed, "Was not able to register reservation");
            }

            _logger.LogDebug("Successfully added Book {0} to Reading List {1}", request.BookId, request.ReadingListId);
            return (Result.Completed, "Successfully added book to list");
        }
    }
}
