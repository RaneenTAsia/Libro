using Application.Configurations;
using Application.Entities.ReadingLists.Queries;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Application.Entities.ReadingLists.Handlers
{
    public class GetReadingListHandler : IRequestHandler<GetReadingListQuery, (ActionResult, PaginationMetadata)>
    {
        public readonly IReadingListsRepository _readingListsRepository;
        public readonly IReadingListItemsFunctionRepository _readingListItemsFunctionRepository;
        public readonly ILogger<GetReadingListHandler> _logger;
        const int maxPageSize = 10;

        public GetReadingListHandler(IReadingListsRepository readingListsRepository, IReadingListItemsFunctionRepository readingListItemsFunctionRepository, IViewBooksRepository viewBooksRepository, ILogger<GetReadingListHandler> logger)
        {
            _readingListsRepository = readingListsRepository;
            _readingListItemsFunctionRepository = readingListItemsFunctionRepository;
            _logger = logger;
        }
        public async Task<(ActionResult, PaginationMetadata)> Handle(GetReadingListQuery request, CancellationToken cancellationToken)
        {
            if (maxPageSize < request.pageSize)
                request.pageSize = maxPageSize;

            _logger.LogDebug("Check if list {0} exists", request.ReadingListId);
            if (!(await _readingListsRepository.ReadingListExistsAsync(request.ReadingListId)))
            {
                return (new NotFoundObjectResult("Reading list with this id does not exist"), null);
            }

            _logger.LogDebug("Get list {0} items from readingListItemsFunction", request.ReadingListId);
            var items = await _readingListItemsFunctionRepository.GetReadingList(request.ReadingListId);

            _logger.LogDebug("Paginating returning list");
            var totalResultCount = items.Count();

            var paginationMetadata = new PaginationMetadata(totalResultCount, request.pageSize, request.pageNumber);

            var resultToReturn = items.OrderBy(r => r.Title).Skip(paginationMetadata.PageSize * (paginationMetadata.CurrentPage - 1)).Take(paginationMetadata.PageSize).ToList();

            return (new OkObjectResult(resultToReturn), paginationMetadata);
        }
    }
}
