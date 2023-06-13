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
    public class DeleteBookHandler : IRequestHandler<DeleteBookCommand, ActionResult>
    {
        public readonly IBookRepository _bookRepository;
        public readonly ILogger<DeleteBookHandler> _logger;
        public readonly IMapper _mapper;

        public DeleteBookHandler(IBookRepository bookRepository, ILogger<DeleteBookHandler> logger, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ActionResult> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Check if book exists");

           if(! await _bookRepository.BookExistsAsync(request.BookId))
            {
                return new NotFoundObjectResult(("A book with this id does not exist"));
            }

           var book = await _bookRepository.GetBookByIdAsync(request.BookId);

           var result = await _bookRepository.DeleteBookAsync(request.BookId);

            if(result == Result.Failed)
            {
                return new ConflictObjectResult("Book was not successfuky Deleted");
            }

            return new OkObjectResult(_mapper.Map<BookDTO>(book));
        }
    }
}
