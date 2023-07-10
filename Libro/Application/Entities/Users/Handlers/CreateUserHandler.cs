using Application.Configurations;
using Application.DTOs;
using Application.Entities.Users.Commands;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Application.Entities.Users.Handlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, ActionResult>
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

        public async Task<ActionResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.UserExistsByEmailAsync(request.Email))
            {
                return new NotFoundObjectResult("User with this email does not exist");
            }

            var userToAdd = new User { Email = request.Email, Username = request.Username };
            userToAdd.PasswordSalt = PasswordHasher.GenerateSalt();
            userToAdd.PasswordHash = PasswordHasher.ComputeHash(request.Password, userToAdd.PasswordSalt, 3);

            var (user, result) = await _userRepository.CreateUserAsync(userToAdd);

            _logger.LogDebug("Created User with username: {0} and email: {1}", user.Username, user.Email);

            if(result == Result.Failed)
            {
                return new ConflictObjectResult("User was not added");
            }

            var userAdded = _mapper.Map<UserDTO>(user);
            return new OkObjectResult(userAdded);
        }

    }
}
