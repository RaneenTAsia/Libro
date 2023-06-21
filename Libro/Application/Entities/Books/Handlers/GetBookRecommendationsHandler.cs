using Application.Configurations;
using Application.DTOs;
using Application.Entities.Books.Queries;
using Domain.Entities;
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
    public class GetBookRecommendationsHandler : IRequestHandler<GetBookRecommendationsQuery, (List<ViewBooks>, PaginationMetadata)>
    {
        private readonly IBookTransactionRepository _bookTransactionRepository;
        private readonly IBookToGenreRepository _bookToGenreRepository;
        private readonly IViewBooksRepository _viewBookRepository;
        private readonly ILogger<GetBookRecommendationsHandler> _logger;
        const int maxPageSize = 10;

        public GetBookRecommendationsHandler(IBookTransactionRepository bookTransactionRepository, IBookToGenreRepository bookToGenreRepository, IViewBooksRepository viewBooksRepository, ILogger<GetBookRecommendationsHandler> logger)
        {
            _bookTransactionRepository = bookTransactionRepository;
            _bookToGenreRepository = bookToGenreRepository;
            _viewBookRepository = viewBooksRepository;
            _logger = logger;
        }

        public async Task<(List<ViewBooks>, PaginationMetadata)> Handle(GetBookRecommendationsQuery request, CancellationToken cancellationToken)
        {
            if (maxPageSize < request.pageSize)
                request.pageSize = maxPageSize;

            _logger.LogDebug("Get user {0} borrowing history", request.UserId);
            var userBookTransactionHistory = await _bookTransactionRepository.GetUserBorrowingHistoryAsync(request.UserId);
            var books = userBookTransactionHistory.Select(b => b.BookId).ToList();

            _logger.LogDebug("Get Top 2 genres of history");
            var top2Genres = _bookToGenreRepository.GetTop2GenresOfBooks(books);

            _logger.LogDebug("Get unprecedented books with similar genres");
            var recommendedBookIds = new List<int>();
            recommendedBookIds.AddRange(_bookToGenreRepository.GetBookIdsByGenreId((int)top2Genres.ElementAtOrDefault(0)).Select( b => b.BookId).ToList());
            recommendedBookIds.AddRange(_bookToGenreRepository.GetBookIdsByGenreId((int)top2Genres.ElementAtOrDefault(1)).Select( b => b.BookId).ToList());

            recommendedBookIds.RemoveAll(r => books.Contains(r));

            recommendedBookIds = recommendedBookIds.Distinct().ToList();

            _logger.LogDebug("Get recommended books as browseable");
            var recommendedBooks = _viewBookRepository.GetBooksWithIds(recommendedBookIds);

            _logger.LogDebug("Paginate data");
            var totalResultCount = recommendedBooks.Count();

            var paginationMetadata = new PaginationMetadata(totalResultCount, request.pageSize, request.pageNumber);

            var resultToReturn = recommendedBooks.OrderBy(r => r.BookId).Skip(paginationMetadata.PageSize * (paginationMetadata.CurrentPage - 1)).Take(paginationMetadata.PageSize).ToList();

            return (resultToReturn, paginationMetadata);
        }
    }
}
