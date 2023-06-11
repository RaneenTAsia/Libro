using Application.Configurations;
using Application.DTOs;
using Application.Entities.Users.Queries;
using AutoMapper;
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

namespace Application.Entities.Users.Handlers
{
    public class GetUserHistoryHandler : IRequestHandler<GetUserHistoryQuery, (ActionResult, PaginationMetadata)>
    {
        public readonly IUserRepository _userRepository;
        public readonly IUserBorrowingHistoryFunctionRepository _userBorrowingHistoryRepository;
        public readonly ILogger<GetUserHistoryHandler> _logger;
        const int maxPageSize = 10;

        public GetUserHistoryHandler(IUserRepository userRepository, IUserBorrowingHistoryFunctionRepository userBorrowingHistoryRepository, ILogger<GetUserHistoryHandler> logger)
        {
            _userRepository = userRepository;
            _userBorrowingHistoryRepository = userBorrowingHistoryRepository;
            _logger = logger;
        }
        public async Task<(ActionResult, PaginationMetadata)> Handle(GetUserHistoryQuery request, CancellationToken cancellationToken)
        {
            if (maxPageSize < request.pageSize)
                request.pageSize = maxPageSize;

            _logger.LogDebug("Check if user with Id {0} exists)", request.UserId);
            var userExists = await _userRepository.UserExistsByIdAsync(request.UserId);

            if (!userExists)
            {
                return (new NotFoundObjectResult( "User does not exists"),null);
            }

            _logger.LogDebug("Retrieve borrowing history of User with Id {0})", request.UserId);
            var historyList = _userBorrowingHistoryRepository.GetUserBorrowingHistory(request.UserId);

            var totalResultCount = historyList.Count();

            var paginationMetadata = new PaginationMetadata(totalResultCount, request.pageSize, request.pageNumber);

            var resultToReturn = historyList.OrderBy(r => r.BookId).Skip(paginationMetadata.PageSize * (paginationMetadata.CurrentPage - 1)).Take(paginationMetadata.PageSize).ToList();

            return (new OkObjectResult(resultToReturn), paginationMetadata);
        }
    }
}
