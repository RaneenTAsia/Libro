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
    public class AddBookHandler : IRequestHandler<AddBookCommand, (Result,string)>
    {
        public readonly IBookRepository _bookRepository;
        public readonly IBookGenreRepository _bookGenreRepository;
        public readonly IAuthorRepository _authorsRepository;
        public readonly ILogger<AddBookHandler> _logger;
        public readonly IMapper _mapper;

        public AddBookHandler(IBookRepository bookRepository, IBookGenreRepository bookGenreRepository, IAuthorRepository authorRepository, ILogger<AddBookHandler> logger, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _bookGenreRepository = bookGenreRepository;
            _authorsRepository= authorRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<(Result, string)> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Map AddBookCommand to Book Entity");
            var bookToBeAdded = _mapper.Map<Book>(request);

            _logger.LogDebug("Add Genres to BookToBeAdded");
            if (request.Genres != null)
            {
                var genres = await _bookGenreRepository.GetGenresByIdsAsync(request.Genres);
                bookToBeAdded.BookGenres.AddRange(genres);
            }

            _logger.LogDebug("Add Authors to BookToBeAdded");
            if (request.BookAuthors != null)
            {
                var authors = await _authorsRepository.GetAuthorsByIdsAsync(request.BookAuthors);
                bookToBeAdded.Authors.AddRange(authors);
            }

            _logger.LogDebug("Adding Book");
            var result = await _bookRepository.AddBookAsync(bookToBeAdded);

            if(result == Result.Failed)
            {
                _logger.LogDebug("Failed to add book");
                return (Result.Failed, "Was not able to Add Book");
            }

            _logger.LogDebug("Successfuly added book");

            return (Result.Completed, "Successfully Added Book");
        }
    }
}
