using Application.Configurations;
using Application.Entities.Books.Queries;
using Domain.Entities;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Books.Handlers
{
    public class TrackOverdueBooksHandler : IRequestHandler<TrackOverdueBooksQuery, (List<ViewOverdueBookDetails>, PaginationMetadata)>
    {
        public readonly IViewOverdueBooksDetailsRepository _viewOverdueBooksDetailsRepository;
        public readonly ILogger<TrackOverdueBooksHandler> _logger;
        const int maxPageSize = 10;

        public TrackOverdueBooksHandler(IViewOverdueBooksDetailsRepository viewOverdueBooksDetailsRepository, ILogger<TrackOverdueBooksHandler> logger)
        {
            _logger = logger;
            _viewOverdueBooksDetailsRepository = viewOverdueBooksDetailsRepository;
        }

        public async Task<(List<ViewOverdueBookDetails>, PaginationMetadata)> Handle(TrackOverdueBooksQuery request, CancellationToken cancellationToken)
        {
            if (maxPageSize < request.pageSize)
                request.pageSize = maxPageSize;

            _logger.LogDebug("Get Overdue book details from ViewOverdueBooksDetails View");
            var overdueBooks = await _viewOverdueBooksDetailsRepository.GetOverdueBooksAsync();

            var totalResultCount = overdueBooks.Count();

            var paginationMetadata = new PaginationMetadata(totalResultCount, request.pageSize, request.pageNumber);

            var resultToReturn = overdueBooks.OrderBy(r => r.BookId).Skip(paginationMetadata.PageSize * (paginationMetadata.CurrentPage - 1)).Take(paginationMetadata.PageSize).ToList();

            return (resultToReturn,paginationMetadata);
        }

    }
}
