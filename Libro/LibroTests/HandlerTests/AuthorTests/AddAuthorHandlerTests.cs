using Application.Entities.Authors.Commands;
using Application.Entities.Authors.Handlers;
using Application.Entities.ReadingLists.Commands;
using Application.Entities.ReadingLists.Handlers;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace LibroTests.HandlerTests.AuthorTests
{
    public class AddAuthorHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly Mock<ILogger<AddAuthorHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AddAuthorHandler _handler;

        public AddAuthorHandlerTests()
        {
            _loggerMock = new Mock<ILogger<AddAuthorHandler>>();
            _mapperMock = new Mock<IMapper>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _authorRepositoryMock = new Mock<IAuthorRepository>();

            _handler = new AddAuthorHandler(
                _bookRepositoryMock.Object,
                _authorRepositoryMock.Object,
                _loggerMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task Handle_AuthorNotAdded_ReturnsFailedResult()
        {
            // Arrange
            var authorToAdd = new Author();
            var command = new AddAuthorCommand();
            var cancellationToken = CancellationToken.None;

            _mapperMock.Setup(m => m.Map<Author>(command)).Returns(authorToAdd);

            _authorRepositoryMock
                .Setup(repo => repo.AddAuthorAsync(authorToAdd))
                .ReturnsAsync(Result.Failed);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("Was not able to Add Author", (result as ConflictObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_AuthorAdded_ReturnsCompletedResult()
        {
            // Arrange
            var command = new AddAuthorCommand
            {
                Name = "Test",
                Description = "Test"
            };


            // Set up AuthorRepository behavior for successful addition
            var authorToBeAdded = new Author();

            _authorRepositoryMock
                .Setup(repo => repo.AddAuthorAsync(authorToBeAdded))
                .ReturnsAsync(Result.Completed);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfully Added Author", (result as OkObjectResult)?.Value);
        }
    }
}

