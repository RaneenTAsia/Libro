using Application.Configurations;
using Application.Entities.Reviews.Commands;
using Application.Entities.Reviews.Queries;
using AutoMapper;
using Domain.Entities;
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

namespace Application.Entities.Reviews.Handlers
{
    public class GetReviewsHandler : IRequestHandler<GetReviewsQuery, (ActionResult, PaginationMetadata)>
    {
        public readonly IBookRepository _bookRepository;
        public readonly IBookReviewsFunctionRepository _bookReviewsFunctionRepository;
        public readonly ILogger<GetReviewsHandler> _logger;
        const int maxPageSize = 10;
        public GetReviewsHandler(IBookRepository bookRepository, IBookReviewsFunctionRepository bookReviewsFunctionRepository, ILogger<GetReviewsHandler> logger)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException();
            _bookReviewsFunctionRepository = bookReviewsFunctionRepository?? throw new ArgumentNullException();
            _logger = logger ?? throw new ArgumentNullException();
        }

        public async Task<(ActionResult, PaginationMetadata)> Handle(GetReviewsQuery request, CancellationToken cancellationToken)
        {
            if (maxPageSize < request.pageSize)
                request.pageSize = maxPageSize;

            _logger.LogDebug("Check if Book {0} exists", request.BookId);
            if (!(await _bookRepository.BookExistsAsync(request.BookId)))
            {
                _logger.LogDebug("Book does not exist");
                return (new NotFoundObjectResult("Book does not exist"), null);
            }

            _logger.LogDebug("Get Book Reviews of Book {1} from fnBookReviews function",  request.BookId);
            var result = await _bookReviewsFunctionRepository.GetBookReviewsAsync(request.BookId);

            _logger.LogDebug("Paginating returning list");
            var totalResultCount = result.Count();

            var paginationMetadata = new PaginationMetadata(totalResultCount, request.pageSize, request.pageNumber);

            var resultToReturn = result.OrderByDescending(r => r.Rating).Skip(paginationMetadata.PageSize * (paginationMetadata.CurrentPage - 1)).Take(paginationMetadata.PageSize).ToList();

            return (new OkObjectResult(resultToReturn), paginationMetadata);
        }
    }
}
