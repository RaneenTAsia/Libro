using Application.Entities.Users.Commands;
using Application.Entities.Users.Handlers;
using Application.Entities.Users.Queries;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Infrastructure.Repositories;
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
    public class GetUserHistoryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUserBorrowingHistoryFunctionRepository> _userBorrowingHistoryRepositoryMock;
        private readonly Mock<ILogger<GetUserHistoryHandler>> _loggerMock;
        private readonly GetUserHistoryHandler _handler;

        public GetUserHistoryHandlerTests()
        {
            _loggerMock = new Mock<ILogger<GetUserHistoryHandler>>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _userBorrowingHistoryRepositoryMock = new Mock<IUserBorrowingHistoryFunctionRepository>();

            _handler = new GetUserHistoryHandler(
                _userRepositoryMock.Object,
                _userBorrowingHistoryRepositoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_NonExistingUser_ReturnsNotFoundObjectResult()
        {
            // Arrange
            var query = new GetUserHistoryQuery
            {
                UserId = 1
            };

            // Set up UserRepository behavior for non-existing user
            _userRepositoryMock
                .Setup(repo => repo.UserExistsByIdAsync(query.UserId))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Item1);
            Assert.Equal("User does not exists", (result.Item1 as NotFoundObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_GetUserHistory_ReturnsOkObjectResult()
        {
            // Arrange
            var query = new GetUserHistoryQuery
            {
                UserId = 1
            };

            // Set up UserRepository behavior for non-existing user
            _userRepositoryMock
                .Setup(repo => repo.UserExistsByIdAsync(query.UserId))
                .ReturnsAsync(true);

            _userBorrowingHistoryRepositoryMock
                .Setup(repo => repo.GetUserBorrowingHistory(It.IsAny<int>()))
                .Returns(new List<UserBorrowingHistoryFunctionResult>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result.Item1);
            Assert.IsType<List<UserBorrowingHistoryFunctionResult>>((result.Item1 as OkObjectResult)?.Value);
        }
    }
}
