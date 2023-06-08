using Application.Configurations;
using Application.DTOs;
using Application.Entities.Users.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Users.Handlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, (UserDTO, Result)>
    {
        public readonly IUserRepository _userRepository;
        public readonly IMapper _mapper;
        public readonly ILogger<CreateUserHandler> _logger;

        public CreateUserHandler(IUserRepository userRepository, IMapper mapper, ILogger<CreateUserHandler> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<(UserDTO, Result)> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.UserExistsByEmailAsync(request.Email))
            {
                return (null, Result.Failed);
            }

            var userToAdd = new User { Email = request.Email, Username = request.Username };
            userToAdd.PasswordSalt = PasswordHasher.GenerateSalt();
            userToAdd.PasswordHash = PasswordHasher.ComputeHash(request.Password, userToAdd.PasswordSalt, 3);

            var (user, result) = await _userRepository.CreateUserAsync(userToAdd);

            _logger.LogDebug("Created User with username: {0} and email: {user.Email}", user.Username);

            var userAdded = _mapper.Map<UserDTO>(user);
            return (userAdded, result);
        }

    }
}
