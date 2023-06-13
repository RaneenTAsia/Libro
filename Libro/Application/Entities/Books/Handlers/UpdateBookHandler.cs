using Application.DTOs;
using Application.Entities.Books.Commands;
using AutoMapper;
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

namespace Application.Entities.Books.Handlers
{
    public class UpdateBookHandler : IRequestHandler<UpdateBookCommand, ActionResult>
    {
        public readonly IBookRepository _bookRepository;
        public readonly IBookGenreRepository _bookGenreRepository;
        public readonly IAuthorRepository _authorsRepository;
        public readonly ILogger<UpdateBookHandler> _logger;
        public readonly IMapper _mapper;

        public UpdateBookHandler(IBookRepository bookRepository, IBookGenreRepository bookGenreRepository, IAuthorRepository authorRepository, ILogger<UpdateBookHandler> logger, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _bookGenreRepository = bookGenreRepository;
            _authorsRepository = authorRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ActionResult> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Check if book with Id {0} exists", request.BookId);
            if (!(await _bookRepository.BookExistsAsync(request.BookId)))
            {
                return new NotFoundObjectResult("Book does not exists");
            }

            _logger.LogDebug("Retrieve book with Id {0} ", request.BookId);
            var bookFromRepo = await _bookRepository.GetBookByIdAsync(request.BookId);

            var bookUpdate = _mapper.Map<BookUpdateDTO>(request.RetrievedBookDTO);

            _logger.LogDebug("Add Genres to bookUpdate");
            if (request.RetrievedBookDTO.Genres != null)
            {
                bookFromRepo.BookGenres.Clear();
                var genres = await _bookGenreRepository.GetGenresByIdsAsync(request.RetrievedBookDTO.Genres);
                bookUpdate.BookGenres.AddRange(genres);
            }

            _logger.LogDebug("Add Authors to bookUpdate");
            if (request.RetrievedBookDTO.BookAuthors != null)
            {
                bookFromRepo.Authors.Clear();
                var authors = await _authorsRepository.GetAuthorsByIdsAsync(request.RetrievedBookDTO.BookAuthors);
                bookUpdate.Authors.AddRange(authors);
            }

            _logger.LogDebug("Map the bookUpdate to the bookFromRepo");
            var result = _mapper.Map(bookUpdate, bookFromRepo);

            try
            {
                await _bookRepository.SaveChangesAsync();
            }
            catch(Exception  e)
            {
                throw e;
            }

            _logger.LogDebug("Successfully Updated Book");
            return new OkObjectResult("Object was successfully updated");
        }
    }
}
