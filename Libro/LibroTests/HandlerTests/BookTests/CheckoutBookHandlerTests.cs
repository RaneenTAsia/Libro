using Application.DTOs;
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
using System.Linq.Expressions;

namespace LibroTests.HandlerTests.BookTests
{
    public class CheckoutBookHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IBookReservationRepository> _bookReservationRepositoryMock;
        private readonly Mock<IBookTransactionRepository> _bookTransactionRepositoryMock;
        private readonly Mock<ILogger<CheckoutBookHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CheckoutBookHandler _handler;

        public CheckoutBookHandlerTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _bookReservationRepositoryMock = new Mock<IBookReservationRepository>();
            _bookTransactionRepositoryMock = new Mock<IBookTransactionRepository>();
            _loggerMock = new Mock<ILogger<CheckoutBookHandler>>();
            _mapperMock = new Mock<IMapper>();

            _handler = new CheckoutBookHandler(
                _bookRepositoryMock.Object,
                _bookReservationRepositoryMock.Object,
                _bookTransactionRepositoryMock.Object,
                _loggerMock.Object,
                _mapperMock.Object
                );
        }

        [Fact]
        public async Task Handle_CountGreaterThan4_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var command = new CheckoutBookCommand
            {
                BookId = 1,
                UserId = 1
            };

            _bookTransactionRepositoryMock
                .Setup(repo => repo.BookTransactionCurrentCountOfUserByIdAsync(command.UserId))
                .ReturnsAsync(5);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Already have maximum amount of books checked out", (result as BadRequestObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_BookDoesNotExist_ReturnsNotFoundObjectResult()
        {
            // Arrange
            var command = new CheckoutBookCommand
            {
                BookId = 1,
                UserId = 1
            };

            _bookTransactionRepositoryMock
                .Setup(repo => repo.BookTransactionCurrentCountOfUserByIdAsync(command.UserId))
                .ReturnsAsync(2);

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
        public async Task Handle_BookNotAvailable_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var command = new CheckoutBookCommand
            {
                BookId = 1,
                UserId = 1
            };

            var book = new Book
            {
                BookStatus = (int)Status.Borrowed
            };

            _bookTransactionRepositoryMock
                .Setup(repo => repo.BookTransactionCurrentCountOfUserByIdAsync(command.UserId))
                .ReturnsAsync(2);

            _bookRepositoryMock
                .Setup(repo => repo.GetBookByIdAsync(command.BookId))
                .ReturnsAsync(book);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Book Not Available for checkout", (result as BadRequestObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_BookReservedBySomeoneElse_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var command = new CheckoutBookCommand
            {
                BookId = 1,
                UserId = 1
            };

            var book = new Book
            {
                BookStatus = (int)Status.Reserved
            };

            _bookTransactionRepositoryMock
                .Setup(repo => repo.BookTransactionCurrentCountOfUserByIdAsync(command.UserId))
                .ReturnsAsync(2);

            _bookRepositoryMock
                .Setup(repo => repo.GetBookByIdAsync(command.BookId))
                .ReturnsAsync(book);

            _bookReservationRepositoryMock
                .Setup(repo => repo.GetBookReservation(command.UserId, command.BookId))
                .Returns((BookReservation)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Book is reserved by someone else", (result as BadRequestObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_BookReservationNotDeleted_ReturnsConflictObjectResult()
        {
            // Arrange
            var command = new CheckoutBookCommand
            {
                BookId = 1,
                UserId = 1
            };

            var book = new Book
            {
                BookStatus = (int)Status.Reserved
            };

            var reservation = new BookReservation();

            _bookTransactionRepositoryMock
                .Setup(repo => repo.BookTransactionCurrentCountOfUserByIdAsync(command.UserId))
                .ReturnsAsync(2);

            _bookRepositoryMock
                .Setup(repo => repo.GetBookByIdAsync(command.BookId))
                .ReturnsAsync(book);

            _bookReservationRepositoryMock
                .Setup(repo => repo.GetBookReservation(command.UserId, command.BookId))
                .Returns(reservation);

            _bookReservationRepositoryMock
                .Setup(repo => repo.DeleteBookReservationAsync(reservation))
                .ReturnsAsync(Result.Failed);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("Could not delete reservation", (result as ConflictObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_BookTransactionAdded_ReturnsOkObjectResult()
        {
            // Arrange
            var command = new CheckoutBookCommand
            {
                BookId = 1,
                UserId = 1
            };

            var book = new Book
            {
                BookStatus = (int)Status.Available
            };

            var reservation = new BookReservation();

            var transaction = new BookTransaction();

            var transactionReturned = new BookTransaction();

            var checkout = new TransactionToReturnForCheckoutDTO();
            _bookRepositoryMock
                .Setup(repo => repo.GetBookByIdAsync(command.BookId))
                .ReturnsAsync(book);

            _bookTransactionRepositoryMock
                .Setup(repo => repo.AddBookTransactionAsync(transaction))
                .ReturnsAsync((transactionReturned, Result.Failed));

            _mapperMock.Setup(repo => repo.Map<TransactionToReturnForCheckoutDTO>(transactionReturned))
                .Returns(checkout);
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
