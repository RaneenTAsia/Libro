﻿using Application.Entities.Users.Commands;
using AutoMapper;
using Domain.Enums;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Users.Handlers
{
    public class ChangeUserRoleHandler : IRequestHandler<ChangeUserRoleCommand, (Result, string)>
    {
        public readonly IUserRepository _userRepository;
        public readonly ILogger<ChangeUserRoleHandler> _logger;

        public ChangeUserRoleHandler(IUserRepository userRepository, ILogger<ChangeUserRoleHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<(Result, string)> Handle(ChangeUserRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId);

            string resultMessage;
            if (user == null)
            {
                _logger.LogDebug("User with UserId {0} does not exist", request.UserId);
                return (Result.Failed, $"User with userId {request.UserId} does not exist");
            }

            if (user.Role == (Role)request.Role)
            {
                resultMessage = "User already has this role";
                _logger.LogDebug(resultMessage);
                return (Result.Failed, resultMessage);
            }

            _logger.LogDebug("User with UserId {0} has role {1} before update", user.UserId, user.Role);

            user.Role = (Role)request.Role;

            await _userRepository.SaveChangesAsync();

            _logger.LogDebug("User with UserId {0} has role {1} after update", user.UserId, user.Role);

            return (Result.Completed, "Successfulyy changed role");
        }
    }
}