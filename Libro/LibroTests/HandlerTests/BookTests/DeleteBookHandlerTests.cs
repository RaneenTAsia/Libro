using Application.DTOs;
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
    public class DeleteBookHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<ILogger<DeleteBookHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly DeleteBookHandler _handler;

        public DeleteBookHandlerTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _loggerMock = new Mock<ILogger<DeleteBookHandler>>();
            _mapperMock = new Mock<IMapper>();

            _handler = new DeleteBookHandler(
                _bookRepositoryMock.Object,
                _loggerMock.Object,
                _mapperMock.Object
                );
        }

        [Fact]
        public async Task Handle_BookDoesNotExist_ReturnsNotFoundObjectResult()
        {
            // Arrange
            var command = new DeleteBookCommand
            {
                BookId = 1,
            };

            var book = new Book();

            _bookRepositoryMock
                .Setup(repo => repo.BookExistsAsync(command.BookId))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("A book with this id does not exist", (result as NotFoundObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_BookNotDeleted_ReturnsConflictObjectResult()
        {
            // Arrange
            var command = new DeleteBookCommand
            {
                BookId = 1,
            };

            var book = new Book();

            _bookRepositoryMock
                .Setup(repo => repo.BookExistsAsync(command.BookId))
                .ReturnsAsync(true);

            _bookRepositoryMock
                .Setup(repo => repo.GetBookByIdAsync(command.BookId))
                .ReturnsAsync(book);

            _bookRepositoryMock
                .Setup(repo => repo.DeleteBookAsync(command.BookId))
                .ReturnsAsync(Result.Failed);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("Book was not successfuky Deleted", (result as ConflictObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_BookDeleted_ReturnsOkObjectResult()
        {
            // Arrange
            var command = new DeleteBookCommand
            {
                BookId = 1,
            };

            var book = new Book();

            var bookDto = new BookDTO();

            _bookRepositoryMock
                .Setup(repo => repo.BookExistsAsync(command.BookId))
                .ReturnsAsync(true);

            _bookRepositoryMock
                .Setup(repo => repo.GetBookByIdAsync(command.BookId))
                .ReturnsAsync(book);

            _bookRepositoryMock
                .Setup(repo => repo.DeleteBookAsync(command.BookId))
                .ReturnsAsync(Result.Completed);

            _mapperMock
                .Setup(repo => repo.Map<BookDTO>(book))
                .Returns(bookDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<BookDTO>((result as OkObjectResult)?.Value);
        }
    }
}
