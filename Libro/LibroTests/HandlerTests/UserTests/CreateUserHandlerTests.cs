using Application.DTOs;
using Application.Entities.Users.Commands;
using Application.Entities.Users.Handlers;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibroTests.HandlerTests.UserTests
{
    public class CreateUserHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILogger<CreateUserHandler>> _loggerMock;
        private readonly CreateUserHandler _handler;

        public CreateUserHandlerTests()
        {
            _loggerMock = new Mock<ILogger<CreateUserHandler>>();
            _mapper = new Mock<IMapper>();
            _userRepositoryMock = new Mock<IUserRepository>();

            _handler = new CreateUserHandler(
                _userRepositoryMock.Object,
                _mapper.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_UserWithThisEmailAlreadyExists_ReturnsFailedResult()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Email = "Test",
                Password = "Test",
                Username = "Test",
            };

            // Set up UserRepository behavior for non-existing user
            _userRepositoryMock
                .Setup(repo => repo.UserExistsByEmailAsync(command.Email))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User with this email does not exist", (result as NotFoundObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_UserNotAdded_ReturnsFailedResult()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Email = "Test",
                Password = "Test",
                Username = "Test",
            };

            // Set up UserRepository behavior for non-existing user
            _userRepositoryMock
                .Setup(repo => repo.UserExistsByEmailAsync(command.Email))
                .ReturnsAsync(false);

            _userRepositoryMock
                .Setup(repo => repo.CreateUserAsync(It.IsAny<User>()))
                .ReturnsAsync((new User(), Result.Failed));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("User was not added", (result as ConflictObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_UserCreated_ReturnsFailedResult()
        {
            // Arrange
            var command = new CreateUserCommand
            {
                Email = "Test",
                Password = "Test",
                Username = "Test",
            };

            // Set up UserRepository behavior for non-existing user
            _userRepositoryMock
                .Setup(repo => repo.UserExistsByEmailAsync(command.Email))
                .ReturnsAsync(false);

            _userRepositoryMock
                .Setup(repo => repo.CreateUserAsync(It.IsAny<User>()))
                .ReturnsAsync((new User(), Result.Completed));

            _mapper
                .Setup(repo => repo.Map<UserDTO>(It.IsAny<User>()))
                .Returns(new UserDTO());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<UserDTO>((result as OkObjectResult)?.Value);
        }
    }
}
