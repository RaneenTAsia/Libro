using Application.Entities.Books.Commands;
using Application.Entities.Books.Handlers;
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

namespace LibroTests.HandlerTests.BookTests
{
    public class SaveBookHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IReadingItemsRepository> _readingItemsRepositoryMock;
        private readonly Mock<ILogger<SaveBookHandler>> _loggerMock;
        private readonly SaveBookHandler _handler;

        public SaveBookHandlerTests()
        {
            _loggerMock = new Mock<ILogger<SaveBookHandler>>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _readingItemsRepositoryMock = new Mock<IReadingItemsRepository>();

            _handler = new SaveBookHandler(
                _bookRepositoryMock.Object,
                _readingItemsRepositoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_BookDoesNotExist_ReturnsFailedResult()
        {
            // Arrange
            var command = new SaveBookCommand
            {
                BookId = 1,
                ReadingListId = 1
            };

            _bookRepositoryMock
                .Setup(repo => repo.BookExistsAsync(command.BookId))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Book Does Not Exist", (result as NotFoundObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_BookAlreadyInList_ReturnsFailedResult()
        {
            // Arrange
            var command = new SaveBookCommand
            {
                BookId = 1,
                ReadingListId = 1
            };

            _bookRepositoryMock
                .Setup(repo => repo.BookExistsAsync(command.BookId))
                .ReturnsAsync(true);

            _readingItemsRepositoryMock
                .Setup(repo => repo.BookExistsInListAsync(command.BookId, command.ReadingListId))
                .ReturnsAsync(true);
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("Book already in List", (result as ConflictObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_BookWasNotAddedToList_ReturnsFailedResult()
        {
            // Arrange
            var command = new SaveBookCommand
            {
                BookId = 1,
                ReadingListId = 1
            };

            _bookRepositoryMock
                .Setup(repo => repo.BookExistsAsync(command.BookId))
                .ReturnsAsync(true);

            _readingItemsRepositoryMock
                .Setup(repo => repo.BookExistsInListAsync(command.BookId, command.ReadingListId))
                .ReturnsAsync(false);

            _readingItemsRepositoryMock
                .Setup(repo => repo.AddBookToReadingList(command.BookId, command.ReadingListId))
                .ReturnsAsync(Result.Failed);
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("Was not able to register reservation", (result as ConflictObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_BookAddedToList_ReturnsCompletedResult()
        {
            // Arrange
            var command = new SaveBookCommand
            {
                BookId = 1,
                ReadingListId = 1
            };

            _bookRepositoryMock
                .Setup(repo => repo.BookExistsAsync(command.BookId))
                .ReturnsAsync(true);

            _readingItemsRepositoryMock
                .Setup(repo => repo.BookExistsInListAsync(command.BookId, command.ReadingListId))
                .ReturnsAsync(false);

            _readingItemsRepositoryMock
                .Setup(repo => repo.AddBookToReadingList(command.BookId, command.ReadingListId))
                .ReturnsAsync(Result.Completed);
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfully added book to list", (result as OkObjectResult)?.Value);
        }
    }
}
