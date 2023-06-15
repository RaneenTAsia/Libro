using Application.Configurations;
using Application.DTOs;
using Application.Entities.ReadingLists.Queries;
using Application.Entities.Users.Handlers;
using AutoMapper;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Application.Entities.ReadingLists.Handlers
{
    public class GetUserReadingListsHandler : IRequestHandler<GetUserReadingListsQuery, (ActionResult, PaginationMetadata)>
    {
        public readonly IUserRepository _userRepository;
        public readonly IReadingListsRepository _readingListsRepository;
        public readonly IMapper _mapper;
        public readonly ILogger<GetUserReadingListsHandler> _logger;
        const int maxPageSize = 10;

        public GetUserReadingListsHandler(IUserRepository userRepository, IReadingListsRepository readingListsRepository, IMapper mapper, ILogger<GetUserReadingListsHandler> logger)
        {
            _userRepository = userRepository;
            _readingListsRepository = readingListsRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<(ActionResult, PaginationMetadata)> Handle(GetUserReadingListsQuery request, CancellationToken cancellationToken)
        {
            if (maxPageSize < request.pageSize)
                request.pageSize = maxPageSize;

            _logger.LogDebug("Check if user with Id {0} exists)", request.UserId);
            var userExists = await _userRepository.UserExistsByIdAsync(request.UserId);

            if (!userExists)
            {
                return (new NotFoundObjectResult("User does not exists"), null);
            }

            _logger.LogDebug("Retrieve ReadingLists of User with Id {0})", request.UserId);
            var readingLists = _readingListsRepository.GetReadingListOfUser(request.UserId);

            var totalResultCount = readingLists.Count();

            var paginationMetadata = new PaginationMetadata(totalResultCount, request.pageSize, request.pageNumber);

            var resultToReturn = readingLists.OrderBy(r => r.ReadingListId).Skip(paginationMetadata.PageSize * (paginationMetadata.CurrentPage - 1)).Take(paginationMetadata.PageSize).ToList();

            return (new OkObjectResult(_mapper.Map<List<ReadingListDTO>>(resultToReturn)), paginationMetadata);
        }
    }
}
