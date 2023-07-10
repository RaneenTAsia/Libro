using Application.Entities.Users.Commands;
using Application.Entities.Users.Handlers;
using Application.Entities.Users.Queries;
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
    public class ChangeUserRoleHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ILogger<ChangeUserRoleHandler>> _loggerMock;
        private readonly ChangeUserRoleHandler _handler;

        public ChangeUserRoleHandlerTests()
        {
            _loggerMock = new Mock<ILogger<ChangeUserRoleHandler>>();
            _userRepositoryMock = new Mock<IUserRepository>();

            _handler = new ChangeUserRoleHandler(
                _userRepositoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_NonExistingUser_ReturnsFailedResult()
        {
            // Arrange
            var command = new ChangeUserRoleCommand
            {
                UserId = 1,
                Role = 1
            };

            // Set up UserRepository behavior for non-existing user
            _userRepositoryMock
                .Setup(repo => repo.GetUserByIdAsync(command.UserId))
                .ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal($"User with userId {command.UserId} does not exist", (result as NotFoundObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_UserAlreadyTargetRole_ReturnsFailedResult()
        {
            // Arrange
            var command = new ChangeUserRoleCommand
            {
                UserId = 1,
                Role = 1
            };

            var user = new User
            {
                UserId = 1,
                Role = Role.Administrator
            };

            // Set up UserRepository behavior for non-existing user
            _userRepositoryMock
                .Setup(repo => repo.GetUserByIdAsync(command.UserId))
                .ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User already has this role", (result as BadRequestObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_UserRoleChanged_ReturnsCompletedResult()
        {
            // Arrange
            var command = new ChangeUserRoleCommand
            {
                UserId = 1,
                Role = 1
            };

            var user = new User
            {
                UserId = 1,
                Role = Role.Librarian
            };

            // Set up UserRepository behavior for non-existing user
            _userRepositoryMock
                .Setup(repo => repo.GetUserByIdAsync(command.UserId))
                .ReturnsAsync(user);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfulyy changed role", (result as OkObjectResult)?.Value);
        }
    }
}
