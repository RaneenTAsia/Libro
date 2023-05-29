using Application.DTOs;
using Application.Users.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence.EntityConfigurations;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Handlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, (UserDTO,Result)>
    {
        public readonly IUserRepository _userRepository;
        public readonly IMapper _mapper;
        public readonly ILogger<CreateUserHandler> _logger;

        public CreateUserHandler( IUserRepository userRepository, IMapper mapper, ILogger<CreateUserHandler> logger)
        {
            _userRepository= userRepository;
            _mapper= mapper;
            _logger= logger;
        }

        public async Task<(UserDTO, Result)> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if(await _userRepository.UserExistsByEmailAsync(request.Email))
            {
                return (null, Result.Failed);
            }

            var userToAdd = new User { Email = request.Email, Username = request.Username };
            userToAdd.PasswordSalt = PasswordHasher.GenerateSalt();
            userToAdd.PasswordHash = PasswordHasher.ComputeHash(request.Password, userToAdd.PasswordSalt, 3);

            var (user, result) = await _userRepository.CreateUserAsync(userToAdd);

            _logger.LogInformation($"Created User with username: {user.Username} and email: {user.Email}" );

            var userAdded = _mapper.Map<UserDTO>(user);
            return (userAdded, result);
        }

    }
}
