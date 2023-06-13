using Application.Entities.Authors.Commands;
using Application.Entities.Books.Commands;
using Application.Entities.Books.Handlers;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Authors.Handlers
{
    public class AddAuthorHandler : IRequestHandler<AddAuthorCommand, (Result, string)>
    {
        public readonly IBookRepository _bookRepository;
        public readonly IAuthorRepository _authorsRepository;
        public readonly ILogger<AddAuthorHandler> _logger;
        public readonly IMapper _mapper;

        public AddAuthorHandler(IBookRepository bookRepository, IAuthorRepository authorRepository, ILogger<AddAuthorHandler> logger, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _authorsRepository = authorRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<(Result, string)> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Map AddBookCommand to Book Entity");
            var authorToBeAdded = _mapper.Map<Author>(request);

            _logger.LogDebug("Add Authors to BookToBeAdded");
            if (request.BookIds != null)
            {
                var books = await _bookRepository.GetBooksByIdsAsync(request.BookIds);
                authorToBeAdded.WrittenBooks.AddRange(books);
            }

            _logger.LogDebug("Adding Author");
            var result = await _authorsRepository.AddAuthorAsync(authorToBeAdded);

            if (result == Result.Failed)
            {
                _logger.LogDebug("Failed to add Author");
                return (Result.Failed, "Was not able to Add Author");
            }

            _logger.LogDebug("Successfuly added Author");

            return (Result.Completed, "Successfully Added Author");
        }
    }
}

