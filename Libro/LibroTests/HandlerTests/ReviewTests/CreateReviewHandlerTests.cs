using Application.DTOs;
using Application.Entities.Reviews.Commands;
using Application.Entities.Reviews.Handlers;
using Application.Entities.Users.Commands;
using Application.Entities.Users.Handlers;
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

namespace LibroTests.HandlerTests.ReviewTests
{
    public class CreateReviewHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IReviewRepository> _reviewRepositoryMock;
        private readonly Mock<ILogger<CreateReviewHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateReviewHandler _handler;

        public CreateReviewHandlerTests()
        {
            _loggerMock = new Mock<ILogger<CreateReviewHandler>>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _reviewRepositoryMock = new Mock<IReviewRepository>();
            _mapperMock = new Mock<IMapper>();

            _handler = new CreateReviewHandler(
                _bookRepositoryMock.Object,
                _reviewRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_NonExistingbook_ReturnsNotFoundObjectResult()
        {
            // Arrange
            var command = new CreateReviewCommand
            {
                BookId = 1,
                CreateReviewDTO = new ReviewRetrievalDTO(),
                UserId = 1
            };

            // Set up UserRepository behavior for non-existing user
            _bookRepositoryMock
                .Setup(repo => repo.BookExistsAsync(command.BookId))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Book does not exist", (result as NotFoundObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_UserAlreadyReviewedBook_ReturnsConflictObjectResult()
        {
            // Arrange
            var command = new CreateReviewCommand
            {
                BookId = 1,
                CreateReviewDTO = new ReviewRetrievalDTO(),
                UserId = 1
            };

            // Set up UserRepository behavior for non-existing user
            _bookRepositoryMock
                .Setup(repo => repo.BookExistsAsync(command.BookId))
                .ReturnsAsync(true);

            _reviewRepositoryMock
                .Setup(repo => repo.ReviewExistsAsync(command.UserId, command.BookId))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("User Already reviewed book", (result as ConflictObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_ReviewNotAdded_ReturnsConflictObjectResult()
        {
            // Arrange
            var command = new CreateReviewCommand
            {
                BookId = 1,
                CreateReviewDTO = new ReviewRetrievalDTO(),
                UserId = 1
            };

            var review = new Review
            {
                BookId = 1,
                UserId = 1,
                Rating = Rating.Ok,
                ReviewContent = "It was a decent book"
            };

            // Set up UserRepository behavior for non-existing user
            _bookRepositoryMock
                .Setup(repo => repo.BookExistsAsync(command.BookId))
                .ReturnsAsync(true);

            _reviewRepositoryMock
                .Setup(repo => repo.ReviewExistsAsync(command.UserId, command.BookId))
                .ReturnsAsync(false);

            _mapperMock
                .Setup(m => m.Map<Review>(It.IsAny<ReviewRetrievalDTO>()))
                .Returns(review);

            _reviewRepositoryMock
                .Setup(repo => repo.CreateReviewAsync(review))
                .ReturnsAsync(Result.Failed);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("Did not add Book Review", (result as ConflictObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_ReviewAdded_ReturnsOkObjectResult()
        {
            // Arrange
            var command = new CreateReviewCommand
            {
                BookId = 1,
                CreateReviewDTO = new ReviewRetrievalDTO(),
                UserId = 1
            };

            var review = new Review
            {
                BookId = 1,
                UserId = 1,
                Rating = Rating.Ok,
                ReviewContent = "It was a decent book"
            };

            // Set up UserRepository behavior for non-existing user
            _bookRepositoryMock
                .Setup(repo => repo.BookExistsAsync(command.BookId))
                .ReturnsAsync(true);

            _reviewRepositoryMock
                .Setup(repo => repo.ReviewExistsAsync(command.UserId, command.BookId))
                .ReturnsAsync(false);

            _mapperMock
                .Setup(m => m.Map<Review>(It.IsAny<ReviewRetrievalDTO>()))
                .Returns(review);

            _reviewRepositoryMock
                .Setup(repo => repo.CreateReviewAsync(review))
                .ReturnsAsync(Result.Completed);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfulyy Added Review", (result as OkObjectResult)?.Value);
        }
    }
}
