using Application.Entities.ReadingLists.Commands;
using Application.Entities.ReadingLists.Handlers;
using Application.Entities.Users.Handlers;
using Application.Entities.Users.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace LibroTests.HandlerTests.UserTests
{
    public class AuthenticateUserHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ILogger<AuthenticateUserHandler>> _loggerMock;
        private readonly Mock<IConfiguration> _configuration;
        private readonly AuthenticateUserHandler _handler;

        public AuthenticateUserHandlerTests()
        {
            _loggerMock = new Mock<ILogger<AuthenticateUserHandler>>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _configuration = new Mock<IConfiguration>();

            _handler = new AuthenticateUserHandler(
                _userRepositoryMock.Object,
                _loggerMock.Object,
                _configuration.Object
            );
        }

        [Fact]
        public async Task Handle_NonExistingUser_ReturnsFailedResult()
        {
            // Arrange
            var query = new AuthenticateUserQuery
            {
                Email = "test",
                Password = "test"
            };

            // Set up UserRepository behavior for non-existing user
            _userRepositoryMock
                .Setup(repo => repo.ValidateUserCredentialsAsync(query.Email, query.Password))
                .ReturnsAsync((new User(), Result.Failed));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(Result.Failed, result.Item2);
            Assert.Equal("Incorrect email or password", result.Item1);
        }

        [Fact]
        public async Task Handle_ExistingUser_ReturnsCompletedResult()
        {
            // Arrange
            var query = new AuthenticateUserQuery
            {
                Email = "test",
                Password = "test"
            };

            var user = new User
            {
                UserId = 1,
                Email = "test",
                PasswordHash = "test",
                PasswordSalt = "test",
                Username = "test",
                Role = Role.Patron
            };

            _configuration
                .Setup(c => c["Authentication:SecretForKey"])
                .Returns("thisisthesecretforgeneratingakey(mustbeatleast32bitlong)");

            // Set up UserRepository behavior for non-existing user
            _userRepositoryMock
                .Setup(repo => repo.ValidateUserCredentialsAsync(query.Email, query.Password))
                .ReturnsAsync((user, Result.Completed));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(Result.Completed, result.Item2);
        }
    }
}
