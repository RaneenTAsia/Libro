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
    public class ReturnBookHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IBookTransactionRepository> _bookTransactionRepositoryMock;
        private readonly Mock<ILogger<ReturnBookHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ReturnBookHandler _handler;

        public ReturnBookHandlerTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _bookTransactionRepositoryMock = new Mock<IBookTransactionRepository>();
            _loggerMock = new Mock<ILogger<ReturnBookHandler>>();
            _mapperMock = new Mock<IMapper>();

            _handler = new ReturnBookHandler(
                _bookRepositoryMock.Object,
                _bookTransactionRepositoryMock.Object,
                _loggerMock.Object,
                _mapperMock.Object
                );
        }

        [Fact]
        public async Task Handle_BookDoesNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            var command = new ReturnBookCommand
            {
                BookId = 1
            };

            _bookRepositoryMock
                .Setup(repo => repo.GetBookByIdAsync(command.BookId))
                .ReturnsAsync((Book)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Book Doesnt Exist", (result as NotFoundObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_BookNotCheckedOut_NotFoundObjectResult()
        {
            // Arrange
            var command = new ReturnBookCommand
            {
                BookId = 1
            };

            var book = new Book
            {
                BookStatus = (int)Status.Available
            };

            _bookRepositoryMock
                .Setup(repo => repo.GetBookByIdAsync(command.BookId))
                .ReturnsAsync(book);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Book was never checked out", (result as NotFoundObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_BookNotInReservations_ConflictObjectResult()
        {
            // Arrange
            var command = new ReturnBookCommand
            {
                BookId = 1
            };

            var book = new Book
            {
                BookStatus = (int)Status.Borrowed
            };

            _bookRepositoryMock
                .Setup(repo => repo.GetBookByIdAsync(command.BookId))
                .ReturnsAsync(book);

            _bookTransactionRepositoryMock
                .Setup(repo => repo.OngoingBookTransationByBookIdAsync(command.BookId))
                .ReturnsAsync((BookTransaction)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("database flawed", (result as ConflictObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_BookReserved_OkObjectResult()
        {
            // Arrange
            var command = new ReturnBookCommand
            {
                BookId = 1
            };

            var book = new Book
            {
                BookStatus = (int)Status.Borrowed
            };

            var bookTransaction = new BookTransaction
            {
                BookId = 1
            };

            var bookTransactionToReturn = new TransactionToReturnForBookReturnDTO
            {
                BookId = 1
            };

            _bookRepositoryMock
                .Setup(repo => repo.GetBookByIdAsync(command.BookId))
                .ReturnsAsync(book);

            _bookTransactionRepositoryMock
                .Setup(repo => repo.OngoingBookTransationByBookIdAsync(It.IsAny<int>()))
                .ReturnsAsync(bookTransaction);

            _mapperMock
                .Setup(repo => repo.Map<TransactionToReturnForBookReturnDTO>(It.IsAny<BookTransaction>()))
                .Returns(bookTransactionToReturn);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.IsType<TransactionToReturnForBookReturnDTO>((result as OkObjectResult)?.Value);
        }

    }
}
