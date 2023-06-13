using Application.DTOs;
using Application.Entities.Users.Commands;
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
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Application.Entities.Users.Handlers
{
    public class UpdateUserProfileHandler : IRequestHandler<UpdateUserProfileCommand, ActionResult>
    {
        public readonly IUserRepository _userRepository;
        public readonly IMapper _mapper;
        public readonly ILogger<UpdateUserProfileHandler> _logger;

        public UpdateUserProfileHandler(IUserRepository userRepository, IMapper mapper, ILogger<UpdateUserProfileHandler> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ActionResult> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Check if user with Id {0} exists", request.UserId);
            if (!(await _userRepository.UserExistsByIdAsync(request.UserId)))
            {
                return new NotFoundObjectResult("User Does Not Exist");
            }

            _logger.LogDebug("Checking Authorization");
            if (request.TokenUserId != request.UserId && request.TokenUserRole.Equals(Role.Patron.ToString()))
                return new UnauthorizedResult();

            _logger.LogDebug("Retrieve user with Id {0} ", request.UserId);
            var userFromRepo = await _userRepository.GetUserByIdAsync(request.UserId);

            _logger.LogDebug("Checking Authorization");
            if (userFromRepo.Role == Role.Librarian && request.TokenUserRole.Equals(Role.Librarian.ToString()) && request.TokenUserId != request.UserId)
            {
                return new UnauthorizedResult();
            }

            _logger.LogDebug("Check if Updated email already exists", request.UserId);
            if (await EmailTaken(request, userFromRepo))
            {
                return new ConflictObjectResult("An account with this email already exists");
            }

            var result = _mapper.Map(request.Profile, userFromRepo);

            await _userRepository.SaveChangesAsync();

            _logger.LogDebug("User with Id {0} successfully updated", request.UserId);
            return new OkObjectResult(_mapper.Map<UserDTO>(result));
        }

        private async Task<bool> EmailTaken(UpdateUserProfileCommand request, User userFromRepo)
        {
            return !(userFromRepo.Email.Equals(request.Profile.Email)) && (await _userRepository.UserExistsByEmailAsync(request.Profile.Email));
        }
    }
}
