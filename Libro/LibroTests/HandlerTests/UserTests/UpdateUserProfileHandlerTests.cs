using Application.DTOs;
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
    public class UpdateUserProfileHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ILogger<UpdateUserProfileHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateUserProfileHandler _handler;

        public UpdateUserProfileHandlerTests()
        {
            _loggerMock = new Mock<ILogger<UpdateUserProfileHandler>>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();

            _handler = new UpdateUserProfileHandler(
                _userRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_NonExistingUser_ReturnsNotFoundObjectResult()
        {
            // Arrange
            var command = new UpdateUserProfileCommand
            {
                Profile = new ProfileToUpdateDTO(),
                TokenUserId = 1,
                TokenUserRole = "Patron",
                UserId = 1

            };

            // Set up UserRepository behavior for non-existing user
            _userRepositoryMock
                .Setup(repo => repo.UserExistsByIdAsync(command.UserId))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User Does Not Exist", (result as NotFoundObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_PatronTriesToEditSomeonesProfile_ReturnsUnauthorizedObjectResult()
        {
            // Arrange
            var command = new UpdateUserProfileCommand
            {
                Profile = new ProfileToUpdateDTO(),
                TokenUserId = 2,
                TokenUserRole = "Patron",
                UserId = 1

            };

            // Set up UserRepository behavior for non-existing user
            _userRepositoryMock
                .Setup(repo => repo.UserExistsByIdAsync(command.UserId))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Handle_LibrarianTriesToEditAnotherLibrariansProfile_ReturnsUnAuthorizedResult()
        {
            // Arrange
            var command = new UpdateUserProfileCommand
            {
                Profile = new ProfileToUpdateDTO(),
                TokenUserId = 2,
                TokenUserRole = "Librarian",
                UserId = 1

            };

            var userReturned = new User
            {
                UserId = 1,
                Role = Role.Librarian
            };

            // Set up UserRepository behavior for non-existing user
            _userRepositoryMock
                .Setup(repo => repo.UserExistsByIdAsync(command.UserId))
                .ReturnsAsync(true);

            _userRepositoryMock
                .Setup(repo => repo.GetUserByIdAsync(command.UserId))
                .ReturnsAsync(userReturned);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Handle_UserEmailEditAlreadyUsed_ReturnsConflictObjectResult()
        {
            // Arrange
            var command = new UpdateUserProfileCommand
            {
                Profile = new ProfileToUpdateDTO { Email = "Test", Username = "Test" },
                TokenUserId = 2,
                TokenUserRole = "Librarian",
                UserId = 1

            };

            var userReturned = new User
            {
                UserId = 1,
                Role = Role.Patron,
                Email = "test2"
            };

            // Set up UserRepository behavior for non-existing user
            _userRepositoryMock
                .Setup(repo => repo.UserExistsByIdAsync(command.UserId))
                .ReturnsAsync(true);

            _userRepositoryMock
                .Setup(repo => repo.GetUserByIdAsync(command.UserId))
                .ReturnsAsync(userReturned);

            _userRepositoryMock
                .Setup(repo => repo.UserExistsByEmailAsync(command.Profile.Email))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("An account with this email already exists", (result as ConflictObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_UpdateProfile_ReturnsOkObjectResult()
        {
            // Arrange
            var command = new UpdateUserProfileCommand
            {
                Profile = new ProfileToUpdateDTO { Email = "Test", Username = "Test" },
                TokenUserId = 2,
                TokenUserRole = "Librarian",
                UserId = 1

            };

            var userReturned = new User
            {
                UserId = 1,
                Role = Role.Patron,
                Email = "test2"
            };

            // Set up UserRepository behavior for non-existing user
            _userRepositoryMock
                .Setup(repo => repo.UserExistsByIdAsync(command.UserId))
                .ReturnsAsync(true);

            _userRepositoryMock
                .Setup(repo => repo.GetUserByIdAsync(command.UserId))
                .ReturnsAsync(userReturned);

            _userRepositoryMock
                .Setup(repo => repo.UserExistsByEmailAsync(command.Profile.Email))
                .ReturnsAsync(false);

            _mapperMock
                .Setup(m => m.Map(command.Profile, userReturned))
                .Returns(userReturned);

            _mapperMock
                .Setup(m => m.Map<UserDTO>(userReturned))
                .Returns(new UserDTO());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<UserDTO>((result as OkObjectResult)?.Value);
        }
    }
}
