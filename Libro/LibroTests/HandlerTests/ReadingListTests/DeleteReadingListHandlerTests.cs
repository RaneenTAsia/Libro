using Application.Entities.ReadingLists.Commands;
using Application.Entities.ReadingLists.Handlers;
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

namespace LibroTests.HandlerTests.ReadingListTests
{
    public class DeleteReadingListHandlerTests
    {
        private readonly Mock<IReadingListsRepository> _readingListsRepositoryMock;
        private readonly Mock<ILogger<DeleteReadingListHandler>> _loggerMock;
        private readonly DeleteReadingListHandler _handler;

        public DeleteReadingListHandlerTests()
        {
            _loggerMock = new Mock<ILogger<DeleteReadingListHandler>>();
            _readingListsRepositoryMock = new Mock<IReadingListsRepository>();

            _handler = new DeleteReadingListHandler(
                _readingListsRepositoryMock.Object,
                _loggerMock.Object
            );
        }


        [Fact]
        public async Task Handle_ReadingListDoesNotExist_ReturnsNotFoundObjectResult()
        {
            // Arrange
            var command = new DeleteReadingListCommand
            {
                ReadingListId = 1
            };

            _readingListsRepositoryMock
                .Setup(repo => repo.ReadingListExistsAsync(command.ReadingListId))
                .ReturnsAsync(false);
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("ReadingList does not exist", (result as NotFoundObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_ReadingListNotDeleted_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var command = new DeleteReadingListCommand
            {
                ReadingListId = 1
            };

            _readingListsRepositoryMock
                .Setup(repo => repo.ReadingListExistsAsync(command.ReadingListId))
                .ReturnsAsync(true);

            _readingListsRepositoryMock
                .Setup(repo => repo.DeleteReadingListAsync(command.ReadingListId))
                .ReturnsAsync(Result.Failed);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Did not delete reading list", (result as BadRequestObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_ReadingListDeleted_ReturnsOkObjectResult()
        {
            // Arrange
            var command = new DeleteReadingListCommand
            {
                ReadingListId = 1
            };

            _readingListsRepositoryMock
                .Setup(repo => repo.ReadingListExistsAsync(command.ReadingListId))
                .ReturnsAsync(true);

            _readingListsRepositoryMock
                .Setup(repo => repo.DeleteReadingListAsync(command.ReadingListId))
                .ReturnsAsync(Result.Completed);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfully deleted reading list", (result as OkObjectResult)?.Value);
        }
    }
}
