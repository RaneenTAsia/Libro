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
    public class UpdateBookHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IBookGenreRepository> _bookGenreRepositoryMock;
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly Mock<ILogger<UpdateBookHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateBookHandler _handler;

        public UpdateBookHandlerTests()
        {
            _loggerMock = new Mock<ILogger<UpdateBookHandler>>();
            _mapperMock = new Mock<IMapper>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _bookGenreRepositoryMock = new Mock<IBookGenreRepository>();
            _authorRepositoryMock = new Mock<IAuthorRepository>();

            _handler = new UpdateBookHandler(
                _bookRepositoryMock.Object,
                _bookGenreRepositoryMock.Object,
                _authorRepositoryMock.Object,
                _loggerMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task Handle_BookDoesNotExist_ReturnsNotFoundObjectResult()
        {
            // Arrange
            var bookToAdd = new Book();
            var command = new UpdateBookCommand
            {
                BookId = 1,
                RetrievedBookDTO = new BookRetrievalDTO { Title = "Test" }
            };

            _bookRepositoryMock
                .Setup(m => m.BookExistsAsync(command.BookId))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Book does not exists", (result as NotFoundObjectResult)?.Value);
        }


        [Fact]
        public async Task Handle_BookUpdated_ReturnsOkObjectResult()
        {
            // Arrange
            var bookToUpdate = new Book();
            var bookUpdate = new BookUpdateDTO();
            var command = new UpdateBookCommand
            {
                BookId = 1,
                RetrievedBookDTO = new BookRetrievalDTO { Title = "Test" }
            };

            _bookRepositoryMock
                .Setup(m => m.BookExistsAsync(command.BookId))
                .ReturnsAsync(true);

            _bookRepositoryMock
                .Setup(repo => repo.GetBookByIdAsync(command.BookId))
                .ReturnsAsync(bookToUpdate);

            _mapperMock
                .Setup(repo => repo.Map<BookUpdateDTO>(It.IsAny<BookRetrievalDTO>()))
                .Returns(bookUpdate);

            _mapperMock
                .Setup(repo => repo.Map(bookUpdate, bookToUpdate))
                .Returns(bookToUpdate);
            ;
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Object was successfully updated", (result as OkObjectResult)?.Value);
        }
    }
}
