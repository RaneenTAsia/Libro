using Application.Entities.Books.Commands;
using Application.Entities.Books.Handlers;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Domain.Services;
using Hangfire;
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
    public class ReserveBookHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IBookReservationRepository> _bookReservationRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMailService> _mailService;
        private readonly Mock<IBookReservationJobRepository> _bookReservationJobRepositoryMock;
        private readonly Mock<ILogger<ReserveBookHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ReserveBookHandler _handler;

        public ReserveBookHandlerTests()
        {
            GlobalConfiguration.Configuration
           .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
           .UseSimpleAssemblyNameTypeSerializer()
           .UseRecommendedSerializerSettings()
           .UseInMemoryStorage();

            _bookRepositoryMock = new Mock<IBookRepository>();
            _bookReservationRepositoryMock = new Mock<IBookReservationRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _mailService = new Mock<IMailService>();
            _bookReservationJobRepositoryMock = new Mock<IBookReservationJobRepository>();
            _loggerMock = new Mock<ILogger<ReserveBookHandler>>();
            _mapperMock = new Mock<IMapper>();

            _handler = new ReserveBookHandler(
                _bookRepositoryMock.Object,
                _bookReservationRepositoryMock.Object,
                _userRepositoryMock.Object,
                _mailService.Object,
                _bookReservationJobRepositoryMock.Object,
                _loggerMock.Object,
                _mapperMock.Object
                ); ;
        }

        [Fact]
        public async Task Handle_BookDoesNotExist_ReturnsFailedResult()
        {
            // Arrange
            var command = new ReserveBookCommand
            {
                BookId = 1,
                UserId = 1
            };

            _bookRepositoryMock
                .Setup(repo => repo.GetBookByIdAsync(command.BookId))
                .ReturnsAsync((Book)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(result.Item1, Result.Failed);
            Assert.Equal("Book Does Not Exist", result.Item2);
        }

        [Fact]
        public async Task Handle_BookNotAvailable_ReturnsFailedResult()
        {
            // Arrange
            var command = new ReserveBookCommand
            {
                BookId = 1,
                UserId = 1
            };

            _bookRepositoryMock
                .Setup(repo => repo.GetBookByIdAsync(command.BookId))
                .ReturnsAsync(new Book());

            _bookRepositoryMock
                .Setup(repo => repo.CheckBookIsAvailableAsync(command.BookId))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(result.Item1, Result.Failed);
            Assert.Equal("Book not available to reserve", result.Item2);
        }

        [Fact]
        public async Task Handle_BookNotReserved_ReturnsFailedResult()
        {
            // Arrange
            var command = new ReserveBookCommand
            {
                BookId = 1,
                UserId = 1
            };

            _bookRepositoryMock
                .Setup(repo => repo.GetBookByIdAsync(command.BookId))
                .ReturnsAsync(new Book());

            _bookRepositoryMock
                .Setup(repo => repo.CheckBookIsAvailableAsync(command.BookId))
                .ReturnsAsync(true);

            _bookReservationRepositoryMock
                .Setup(repo => repo.AddBookReservationAsync(It.IsAny<BookReservation>()))
                .ReturnsAsync(Result.Failed);
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(result.Item1, Result.Failed);
            Assert.Equal("Was not able to register reservation", result.Item2);
        }

        [Fact]
        public async Task Handle_BookReserved_ReturnsSuccessResult()
        {
            // Arrange
            var command = new ReserveBookCommand
            {
                BookId = 1,
                UserId = 1
            };

            _bookRepositoryMock
                .Setup(repo => repo.GetBookByIdAsync(command.BookId))
                .ReturnsAsync(new Book());

            _bookRepositoryMock
                .Setup(repo => repo.CheckBookIsAvailableAsync(command.BookId))
                .ReturnsAsync(true);

            _bookReservationRepositoryMock
                .Setup(repo => repo.AddBookReservationAsync(It.IsAny<BookReservation>()))
                .ReturnsAsync(Result.Completed);

            _userRepositoryMock
                .Setup(repo => repo.GetUserByIdAsync(command.UserId))
                .ReturnsAsync(new User { UserId = 1, Email = "test" });

            _bookReservationJobRepositoryMock
                .Setup(repo => repo.AddBookReservationJobAsync(new BookReservationJob()));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(result.Item1, Result.Completed);
            Assert.Equal("Successfully Reserved Book", result.Item2);
        }
    }
}
