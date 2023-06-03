using Application.Abstractions.Repositories;
using Application.DTOs;
using Application.Entities.Users.Commands;
using AutoMapper;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities.Users.Handlers
{
    public class ChangeUserRoleHandler : IRequestHandler<ChangeUserRoleCommand, (Result, string)>
    {
        public readonly IUserRepository _userRepository;
        public readonly IMapper _mapper;
        public readonly ILogger<ChangeUserRoleHandler> _logger;

        public ChangeUserRoleHandler(IUserRepository userRepository, IMapper mapper, ILogger<ChangeUserRoleHandler> logger)
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
                resultMessage = $"User with UserId {request.UserId} does not exist";
                _logger.LogInformation(resultMessage);
                return (Result.Failed, resultMessage);
            }

            if (user.Role == (Role)request.Role)
            {
                resultMessage = $"User already has this role";
                _logger.LogInformation(resultMessage);
                return (Result.Failed, resultMessage);
            }

            _logger.LogInformation($"User with UserId {request.UserId} has role {user.Role} before update");

            user.Role = (Role)request.Role;

            await _userRepository.SaveChangesAsync();

            _logger.LogInformation($"User with UserId {request.UserId} has role {user.Role} after update");

            return (Result.Completed, "Successfulyy changed role");
        }
    }
}
