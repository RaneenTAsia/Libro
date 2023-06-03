using Application.Abstractions.Repositories;
using Application.Configurations;
using Application.DTOs;
using Application.Entities.Books.Queries;
using AutoMapper;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Application.Entities.Books.Handlers
{
    public class SearchBooksHandler : IRequestHandler<SearchBooksQuery, (List<BrowsingBookDTO>, PaginationMetadata)>
    {
        public readonly IViewBooksRepository _viewBookRepository;
        public readonly IBookToGenreRepository _bookToGenreRepository;
        public readonly ILogger<SearchBooksHandler> _logger;
        public readonly IMapper _mapper;
        const int maxPageSize = 10;

        public SearchBooksHandler(IViewBooksRepository viewBookRepository, IBookToGenreRepository bookToGenreRepository, ILogger<SearchBooksHandler> logger, IMapper mapper)
        {
            _viewBookRepository = viewBookRepository;
            _bookToGenreRepository = bookToGenreRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<(List<BrowsingBookDTO>, PaginationMetadata)> Handle(SearchBooksQuery request, CancellationToken cancellationToken)
        {
            if (maxPageSize < request.pageSize)
                request.pageSize = maxPageSize;


            var resultList = await _viewBookRepository.GetBooksAsync();

            _logger.LogInformation($"Execute SearchAuthorBooks Function with value {request.Author}");

            if (request.Author != null)
            {
                var authorList = _viewBookRepository.GetBooksWithAuthor(request.Author);
                resultList = resultList.Where(r => authorList.Any(l => l.BookId == r.BookId)).ToList();
                _logger.LogInformation($"Search ViewBooks for author value {request.Author}");
            }

            if (request.Title != null)
            {
                var titleList = _viewBookRepository.GetBooksWithTitle(request.Title);
                resultList = resultList.Where(r => titleList.Any(l => l.BookId == r.BookId)).ToList(); ;

                _logger.LogInformation($"Search ViewBooks for title value {request.Title}");
            }

            if (request.GenreId != null)
            {

                var list = _bookToGenreRepository.GetBookIdsByGenreId((int)request.GenreId);

                resultList = resultList.Where(r => list.Any(l => l.BookId == r.BookId)).ToList();

                _logger.LogInformation($"Search SearchAuthorBooks Function result for GenreId {request.GenreId}");
            }

            var totalResultCount = resultList.Count();

            var paginationMetadata = new PaginationMetadata(totalResultCount, request.pageSize, request.pageNumber);

            var resultToReturn = resultList.OrderBy(r => r.BookId).Skip(paginationMetadata.PageSize * (paginationMetadata.CurrentPage - 1)).Take(paginationMetadata.PageSize).ToList();

            return (_mapper.Map<List<BrowsingBookDTO>>(resultToReturn), paginationMetadata);
        }
    }
}
