using Application.Entities.Authors.Commands;
using Application.Entities.Authors.Handlers;
using Application.Entities.Books.Commands;
using Application.Entities.Books.Handlers;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace LibroTests.HandlerTests.BookTests
{
    public class AddBookHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IBookGenreRepository> _bookGenreRepositoryMock;
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly Mock<ILogger<AddBookHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AddBookHandler _handler;

        public AddBookHandlerTests()
        {
            _loggerMock = new Mock<ILogger<AddBookHandler>>();
            _mapperMock = new Mock<IMapper>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _bookGenreRepositoryMock = new Mock<IBookGenreRepository>();
            _authorRepositoryMock = new Mock<IAuthorRepository>();

            _handler = new AddBookHandler(
                _bookRepositoryMock.Object,
                _bookGenreRepositoryMock.Object,
                _authorRepositoryMock.Object,
                _loggerMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task Handle_BookNotAdded_ReturnsFailedResult()
        {
            // Arrange
            var bookToAdd = new Book();
            var command = new AddBookCommand();

            _mapperMock.Setup(m => m.Map<Book>(command)).Returns(bookToAdd);

            _bookRepositoryMock
                .Setup(m => m.AddBookAsync(bookToAdd))
                .ReturnsAsync(Result.Failed);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Was not able to Add Book", (result as BadRequestObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_BookAdded_ReturnsCompletedResult()
        {
            // Arrange
            var bookToAdd = new Book();
            var command = new AddBookCommand();

            _mapperMock.Setup(m => m.Map<Book>(command)).Returns(bookToAdd);

            _bookRepositoryMock
                .Setup(m => m.AddBookAsync(bookToAdd))
                .ReturnsAsync(Result.Completed);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfully Added Book", (result as OkObjectResult)?.Value);
        }
    }
}
