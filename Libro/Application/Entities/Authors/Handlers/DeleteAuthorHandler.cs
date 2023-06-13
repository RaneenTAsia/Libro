using Application.DTOs;
using Application.Entities.Authors.Commands;
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

namespace Application.Entities.Authors.Handlers
{
    public class DeleteAuthorHandler : IRequestHandler<DeleteAuthorCommand, ActionResult>
    {
        public readonly IAuthorRepository _authorRepository;
    public readonly ILogger<DeleteAuthorHandler> _logger;
    public readonly IMapper _mapper;

    public DeleteAuthorHandler(IAuthorRepository authorRepository, ILogger<DeleteAuthorHandler> logger, IMapper mapper)
    {
        _authorRepository = authorRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<ActionResult> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Check if author exists");

        if (!await _authorRepository.AuthorExistsAsync(request.AuthorId))
        {
            return new NotFoundObjectResult(("An author with this id does not exist"));
        }

        _logger.LogDebug("Deleting author");
        var authorFromRepo= await _authorRepository.DeleteAuthorAsync(request.AuthorId);

        if (authorFromRepo == null)
        {
            return new ConflictObjectResult("Author was not successfully Deleted");
        }

        _logger.LogDebug("Author successfully deleted");
        return new OkObjectResult(_mapper.Map<AuthorDTO>(authorFromRepo));
    }
}
}
