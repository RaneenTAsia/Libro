using System.Threading;
using Application.Entities.ReadingLists.Commands;
using Application.Entities.ReadingLists.Handlers;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace LibroTests.HandlerTests.ReadingListTests
{
    public class AddReadingListHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IReadingListsRepository> _readingListsRepositoryMock;
        private readonly Mock<ILogger<AddReadingListHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AddReadingListHandler _handler;

        public AddReadingListHandlerTests()
        {
            _loggerMock = new Mock<ILogger<AddReadingListHandler>>();
            _mapperMock = new Mock<IMapper>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _readingListsRepositoryMock = new Mock<IReadingListsRepository>();

            _handler = new AddReadingListHandler(
                _readingListsRepositoryMock.Object,
                _userRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_ExistingUser_ReturnsOkObjectResult()
        {
            // Arrange
            var command = new AddReadingListCommand
            {
                UserId = 1,
                // Set other properties as needed
            };

            // Set up UserRepository behavior for existing user         

            _userRepositoryMock
                .Setup(r => r.UserExistsByIdAsync(command.UserId))
                .ReturnsAsync(true);

            _mapperMock
                .Setup(mapper => mapper.Map<ReadingList>(command))
                .Returns(new ReadingList());

            // Set up ReadingListsRepository behavior for successful addition

            _readingListsRepositoryMock
                .Setup(repo => repo.AddReadingListAsync(It.IsAny<ReadingList>()))
                .ReturnsAsync(Result.Completed);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfully Added reading list", (result as OkObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_NonExistingUser_ReturnsNotFoundObjectResult()
        {
            // Arrange
            var command = new AddReadingListCommand
            {
                UserId = 1,
                // Set other properties as needed
            };

            // Set up UserRepository behavior for non-existing user
            _userRepositoryMock
                .Setup(repo => repo.UserExistsByIdAsync(command.UserId))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("User does not exists", (result as NotFoundObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_FailedToAddReadingList_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var command = new AddReadingListCommand
            {
                UserId = 1,
                // Set other properties as needed
            };

            // Set up UserRepository behavior for existing user
            _userRepositoryMock
                .Setup(repo => repo.UserExistsByIdAsync(command.UserId))
                .ReturnsAsync(true);

            _mapperMock
                .Setup(mapper => mapper.Map<ReadingList>(command))
                .Returns(new ReadingList());

            // Set up ReadingListsRepository behavior for failed addition
            _readingListsRepositoryMock
                .Setup(repo => repo.AddReadingListAsync(It.IsAny<ReadingList>()))
                .ReturnsAsync(Result.Failed);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Did not add reading list", (result as BadRequestObjectResult)?.Value);
        }
    }
}

