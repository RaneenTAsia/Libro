using Application.Entities.ReadingLists.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Application.Entities.ReadingLists.Handlers
{
    public class AddReadingListHandler : IRequestHandler<AddReadingListCommand, ActionResult>
    {
        public readonly IUserRepository _userRepository;
        public readonly IReadingListsRepository _readingListsRepository;
        public readonly ILogger<AddReadingListHandler> _logger;
        public readonly IMapper _mapper;

        public AddReadingListHandler(IReadingListsRepository readingListsRepository, IUserRepository userRepository, IMapper mapper, ILogger<AddReadingListHandler> logger)
        {
            _userRepository = userRepository;
            _readingListsRepository = readingListsRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<ActionResult> Handle(AddReadingListCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Check if user with Id {0} exists)", request.UserId);
            var userExists = await _userRepository.UserExistsByIdAsync(request.UserId);

            if (!userExists)
            {
                return new NotFoundObjectResult("User does not exists");
            }

            var listToAdd = _mapper.Map<ReadingList>(request);

            _logger.LogDebug(" Adding ReadingList");
            var result = await _readingListsRepository.AddReadingListAsync(listToAdd);

            if (result == Result.Failed)
            {
                return new BadRequestObjectResult("Did not add reading list");
            }

            return new OkObjectResult("Successfully Added reading list");
        }
    }


}
