﻿using Application.Entities.Users.Commands;
using AutoMapper;
using Domain.Enums;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Application.Entities.Users.Handlers
{
    public class ChangeUserRoleHandler : IRequestHandler<ChangeUserRoleCommand, ActionResult>
    {
        public readonly IUserRepository _userRepository;
        public readonly ILogger<ChangeUserRoleHandler> _logger;

        public ChangeUserRoleHandler(IUserRepository userRepository, ILogger<ChangeUserRoleHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<ActionResult> Handle(ChangeUserRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId);

            string resultMessage;
            if (user == null)
            {
                _logger.LogDebug("User with UserId {0} does not exist", request.UserId);
                return new NotFoundObjectResult($"User with userId {request.UserId} does not exist");
            }

            if (user.Role == (Role)request.Role)
            {
                resultMessage = "User already has this role";
                _logger.LogDebug(resultMessage);
                return new BadRequestObjectResult(resultMessage);
            }

            _logger.LogDebug("User with UserId {0} has role {1} before update", user.UserId, user.Role);

            user.Role = (Role)request.Role;

            await _userRepository.SaveChangesAsync();

            _logger.LogDebug("User with UserId {0} has role {1} after update", user.UserId, user.Role);

            return new OkObjectResult( "Successfulyy changed role");
        }
    }
}