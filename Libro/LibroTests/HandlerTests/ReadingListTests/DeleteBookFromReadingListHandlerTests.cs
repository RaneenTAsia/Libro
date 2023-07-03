using Application.Entities.ReadingLists.Commands;
using Application.Entities.ReadingLists.Handlers;
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

namespace LibroTests.HandlerTests.ReadingListTests
{
    public class DeleteBookFromReadingListHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IReadingListsRepository> _readingListsRepositoryMock;
        private readonly Mock<IReadingItemsRepository> _readingItemsRepositoryMock;
        private readonly Mock<ILogger<DeleteBookFromReadingListHandler>> _loggerMock;
        private readonly DeleteBookFromReadingListHandler _handler;

        public DeleteBookFromReadingListHandlerTests()
        {
            _loggerMock = new Mock<ILogger<DeleteBookFromReadingListHandler>>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _readingItemsRepositoryMock = new Mock<IReadingItemsRepository>();
            _readingListsRepositoryMock = new Mock<IReadingListsRepository>();

            _handler = new DeleteBookFromReadingListHandler(
                _bookRepositoryMock.Object,
                _readingListsRepositoryMock.Object,
                _readingItemsRepositoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_BookDoesNotExist_ReturnsNotFoundObjectResult()
        {
            // Arrange
            var command = new DeleteBookFromReadingListCommand
            {
                BookId = 1
            };

            _bookRepositoryMock
                .Setup(repo => repo.BookExistsAsync(command.BookId))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Book does not exist", (result as NotFoundObjectResult)?.Value);
        }


        [Fact]
        public async Task Handle_ReadingListDoesNotExist_ReturnsNotFoundObjectResult()
        {
            // Arrange
            var command = new DeleteBookFromReadingListCommand
            {
                BookId = 1,
                ReadingListId = 1
            };

            _bookRepositoryMock
                .Setup(repo => repo.BookExistsAsync(command.BookId))
                .ReturnsAsync(true);

            _readingListsRepositoryMock
                .Setup(repo => repo.ReadingListExistsAsync(command.ReadingListId))
                .ReturnsAsync(false);
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("ReadingList  does not exist", (result as NotFoundObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_BookNotInReadingList_ReturnsNotFoundObjectResult()
        {
            // Arrange
            var command = new DeleteBookFromReadingListCommand
            {
                BookId = 1,
                ReadingListId = 1
            };

            _bookRepositoryMock
                .Setup(repo => repo.BookExistsAsync(command.BookId))
                .ReturnsAsync(true);

            _readingListsRepositoryMock
                .Setup(repo => repo.ReadingListExistsAsync(command.ReadingListId))
                .ReturnsAsync(true);

            _readingItemsRepositoryMock
                .Setup(repo => repo.BookExistsInListAsync(command.BookId, command.ReadingListId))
                .ReturnsAsync(false);
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Book does not exist in readingList", (result as NotFoundObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_BookNotRemovedFromReadingList_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var command = new DeleteBookFromReadingListCommand
            {
                BookId = 1,
                ReadingListId = 1
            };

            _bookRepositoryMock
                .Setup(repo => repo.BookExistsAsync(command.BookId))
                .ReturnsAsync(true);

            _readingListsRepositoryMock
                .Setup(repo => repo.ReadingListExistsAsync(command.ReadingListId))
                .ReturnsAsync(true);

            _readingItemsRepositoryMock
                .Setup(repo => repo.BookExistsInListAsync(command.BookId, command.ReadingListId))
                .ReturnsAsync(true);

            _readingItemsRepositoryMock
                .Setup(repo => repo.DeleteBookFromReadingListAsync(command.BookId, command.ReadingListId))
                .ReturnsAsync(Result.Failed);
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Book was not deleted from reading list", (result as BadRequestObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_BookRemovedFromReadingList_ReturnsOkObjectResult()
        {
            // Arrange
            var command = new DeleteBookFromReadingListCommand
            {
                BookId = 1,
                ReadingListId = 1
            };

            _bookRepositoryMock
                .Setup(repo => repo.BookExistsAsync(command.BookId))
                .ReturnsAsync(true);

            _readingListsRepositoryMock
                .Setup(repo => repo.ReadingListExistsAsync(command.ReadingListId))
                .ReturnsAsync(true);

            _readingItemsRepositoryMock
                .Setup(repo => repo.BookExistsInListAsync(command.BookId, command.ReadingListId))
                .ReturnsAsync(true);

            _readingItemsRepositoryMock
                .Setup(repo => repo.DeleteBookFromReadingListAsync(command.BookId, command.ReadingListId))
                .ReturnsAsync(Result.Completed);
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfully deleted book from reading list", (result as OkObjectResult)?.Value);
        }
    }
}
