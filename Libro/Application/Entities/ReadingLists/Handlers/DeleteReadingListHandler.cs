using Application.Entities.ReadingLists.Commands;
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

namespace Application.Entities.ReadingLists.Handlers
{
    public class DeleteReadingListHandler : IRequestHandler<DeleteReadingListCommand, ActionResult>
    {
        public readonly IReadingListsRepository _readingListsRepository;
        public readonly ILogger<DeleteReadingListHandler> _logger;

        public DeleteReadingListHandler( IReadingListsRepository readingListsRepository, ILogger<DeleteReadingListHandler> logger)
        {
            _readingListsRepository = readingListsRepository;
            _logger = logger;
        }
        public async Task<ActionResult> Handle(DeleteReadingListCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Check if ReadingList {0} exists", request.ReadingListId);
            if (!(await _readingListsRepository.ReadingListExistsAsync(request.ReadingListId)))
            {
                _logger.LogDebug("ReadingList {0} does not exist", request.ReadingListId);
                return new NotFoundObjectResult("ReadingList does not exist");
            }

            _logger.LogDebug(" Deleting ReadingList {0}", request.ReadingListId);
            var result = await _readingListsRepository.DeleteReadingListAsync(request.ReadingListId);

            if (result == Result.Failed)
            {
                return new BadRequestObjectResult("Did not delete reading list");
            }

            return new OkObjectResult("Successfully deleted reading list");
        }
    }
}
