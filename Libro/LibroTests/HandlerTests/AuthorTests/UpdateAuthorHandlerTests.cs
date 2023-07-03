using Application.DTOs;
using Application.Entities.Authors.Commands;
using Application.Entities.Authors.Handlers;
using AutoMapper;
using Domain.Entities;
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
    public class UpdateAuthorHandlerTests
    {
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<ILogger<UpdateAuthorHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateAuthorHandler _handler;

        public UpdateAuthorHandlerTests()
        {
            _loggerMock = new Mock<ILogger<UpdateAuthorHandler>>();
            _mapperMock = new Mock<IMapper>();
            _authorRepositoryMock = new Mock<IAuthorRepository>();
            _bookRepositoryMock = new Mock<IBookRepository>();

            _handler = new UpdateAuthorHandler(
                _bookRepositoryMock.Object,
                _authorRepositoryMock.Object,
                _loggerMock.Object,
                _mapperMock.Object
                );
        }

        [Fact]
        public async Task Handle_AuthorDoesNotExist_ReturnsNotFoundObjectResult()
        {
            // Arrange
            var command = new UpdateAuthorCommand
            {
                AuthorId = 1,
            };

            _authorRepositoryMock
                .Setup(repo => repo.AuthorExistsAsync(command.AuthorId))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Author does not exist", (result as NotFoundObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_AuthorUpdate_ReturnsOkObjectResult()
        {
            // Arrange
            var command = new UpdateAuthorCommand
            {
                AuthorId = 1,
                RetrievedAuthorDTO = new AuthorRetrievalDTO()
            };

            var authorToUpdate = new Author();
            var authorUpdate = new AuthorUpdateDTO();

            _authorRepositoryMock
                .Setup(repo => repo.AuthorExistsAsync(command.AuthorId))
                .ReturnsAsync(true);

            _authorRepositoryMock
                .Setup(repo => repo.GetAuthorByIdAsync(command.AuthorId))
                .ReturnsAsync(authorToUpdate);


            _mapperMock
                .Setup(m => m.Map<AuthorUpdateDTO>(command.RetrievedAuthorDTO))
                .Returns(authorUpdate);

            _mapperMock
                .Setup(m => m.Map(authorUpdate, authorToUpdate))
                .Returns(authorToUpdate);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Object was successfully updated", (result as OkObjectResult)?.Value);
        }
    }
}
