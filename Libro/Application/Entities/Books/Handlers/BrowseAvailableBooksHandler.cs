using Application.Abstractions.Repositories;
using Application.Configurations;
using Application.DTOs;
using Application.Entities.Books.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Books.Handlers
{
    public class BrowseAvailableBooksHandler : IRequestHandler<BrowseAvailableBooksQuery, (List<BrowsingBookDTO>,PaginationMetadata)>
    {
        public readonly IViewBooksRepository _viewBookRepository;
        public readonly ILogger<BrowseAvailableBooksHandler> _logger;
        public readonly IMapper _mapper;
        const int maxPageSize = 10;

        public BrowseAvailableBooksHandler(IViewBooksRepository viewBookRepository, ILogger<BrowseAvailableBooksHandler> logger, IMapper mapper)
        {
            _viewBookRepository = viewBookRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<(List<BrowsingBookDTO>,PaginationMetadata)> Handle(BrowseAvailableBooksQuery request, CancellationToken cancellationToken)
        {
            if (maxPageSize < request.pageSize)
                request.pageSize = maxPageSize;


            var resultList = await _viewBookRepository.GetBooksAsync();
            _logger.LogInformation($"Select ViewBooks View");

            resultList = resultList.Where(r => r.BookStatus == Status.Available).ToList();
            _logger.LogInformation($"Filter for available books in view result");

            var totalResultCount = resultList.Count();

            var paginationMetadata = new PaginationMetadata(totalResultCount, request.pageSize, request.pageNumber);

            var resultToReturn = resultList.OrderBy(r => r.BookId).Skip(paginationMetadata.PageSize * (paginationMetadata.CurrentPage - 1)).Take(paginationMetadata.PageSize).ToList();

            return (_mapper.Map<List<BrowsingBookDTO>>(resultToReturn), paginationMetadata);
        }
    }
}
