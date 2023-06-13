using Application.DTOs;
using Application.Entities.Authors.Commands;
using AutoMapper;
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
    public class UpdateAuthorHandler : IRequestHandler<UpdateAuthorCommand, ActionResult>
    {
        public readonly IBookRepository _bookRepository;
        public readonly IAuthorRepository _authorRepository;
        public readonly ILogger<UpdateAuthorHandler> _logger;
        public readonly IMapper _mapper;

        public UpdateAuthorHandler(IBookRepository bookRepository, IAuthorRepository authorRepository, ILogger<UpdateAuthorHandler> logger, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ActionResult> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Check if author with Id {0} exists", request.AuthorId);
            if (!(await _authorRepository.AuthorExistsAsync(request.AuthorId)))
            {
                return new NotFoundObjectResult("Author does not exist");
            }

            _logger.LogDebug("Retrieve Author with Id {0} ", request.AuthorId);
            var authorFromRepo = await _authorRepository.GetAuthorByIdAsync(request.AuthorId);

            var authorUpdate = _mapper.Map<AuthorUpdateDTO>(request.RetrievedAuthorDTO);

            if (request.RetrievedAuthorDTO.BookIds != null)
            {
                _logger.LogDebug("Add Books to authorUpdate");
                authorFromRepo.WrittenBooks.Clear();
                var books = await _bookRepository.GetBooksByIdsAsync(request.RetrievedAuthorDTO.BookIds);
                authorFromRepo.WrittenBooks.AddRange(books);
            }

            _logger.LogDebug("Map the authorUpdate to the authorFromRepo");
            var result = _mapper.Map(authorUpdate, authorFromRepo);

            try
            {
                await _authorRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }

            _logger.LogDebug("Successfully Updated Book");
            return new OkObjectResult("Object was successfully updated");
        }
    }
}
