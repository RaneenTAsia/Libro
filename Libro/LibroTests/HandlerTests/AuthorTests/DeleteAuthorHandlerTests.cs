using Application.DTOs;
using Application.Entities.Authors.Commands;
using Application.Entities.Authors.Handlers;
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

namespace LibroTests.HandlerTests.AuthorTests
{
    public class DeleteAuthorHandlerTests
    {
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly Mock<ILogger<DeleteAuthorHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly DeleteAuthorHandler _handler;

        public DeleteAuthorHandlerTests()
        {
            _loggerMock = new Mock<ILogger<DeleteAuthorHandler>>();
            _mapperMock = new Mock<IMapper>();
            _authorRepositoryMock = new Mock<IAuthorRepository>();

            _handler = new DeleteAuthorHandler(
                _authorRepositoryMock.Object,
                _loggerMock.Object,
                _mapperMock.Object
                );
        }

        [Fact]
        public async Task Handle_AuthorNotDeleted_ReturnsConflictObjectResult()
        {
            // Arrange
            var command = new DeleteAuthorCommand
            {
                AuthorId = 1
            };

            var authorToDelete = new Author();

            _authorRepositoryMock
                .Setup(repo => repo.AuthorExistsAsync(command.AuthorId))
                .ReturnsAsync(true);

            _authorRepositoryMock
                .Setup(repo => repo.DeleteAuthorAsync(command.AuthorId))
                .ReturnsAsync((Author)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("Author was not successfully Deleted", (result as ConflictObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_AuthorDeleted_ReturnsOkObjectResult()
        {
            // Arrange
            var command = new DeleteAuthorCommand
            {
                AuthorId = 1
            };

            var authorToDelete = new Author();

            _authorRepositoryMock
                .Setup(repo => repo.AuthorExistsAsync(command.AuthorId))
                .ReturnsAsync(true);

            _authorRepositoryMock
                .Setup(repo => repo.DeleteAuthorAsync(command.AuthorId))
                .ReturnsAsync(authorToDelete);

            _mapperMock
                .Setup(m => m.Map<AuthorDTO>(authorToDelete))
                .Returns(new AuthorDTO());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<AuthorDTO>((result as OkObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_AuthorDoesNotExist_ReturnsNotFoundObjectResult()
        {
            // Arrange
            var command = new DeleteAuthorCommand
            {
                AuthorId = 1
            };

            _authorRepositoryMock
                .Setup(repo => repo.AuthorExistsAsync(command.AuthorId))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("An author with this id does not exist", (result as NotFoundObjectResult)?.Value);
        }
    }
}
