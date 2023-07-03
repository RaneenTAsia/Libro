using Application.DTOs;
using Application.Entities.Reviews.Commands;
using Application.Entities.Reviews.Handlers;
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
    public class UpdateReviewHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IReviewRepository> _reviewRepositoryMock;
        private readonly Mock<ILogger<UpdateReviewHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UpdateReviewHandler _handler;

        public UpdateReviewHandlerTests()
        {
            _loggerMock = new Mock<ILogger<UpdateReviewHandler>>();
            _mapperMock = new Mock<IMapper>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _reviewRepositoryMock = new Mock<IReviewRepository>();

            _handler = new UpdateReviewHandler(
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
            var command = new UpdateReviewCommand
            {
                BookId = 1,
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
        public async Task Handle_UserNeverReviewedBook_ReturnsConflictObjectResult()
        {
            // Arrange
            var command = new UpdateReviewCommand
            {
                BookId = 1,
                UserId = 1,
                CreateReviewDTO = new ReviewRetrievalDTO { Rating = Rating.Bad, ReviewContent = " It was a horribly written awkward commedy" }
            };

            // Set up UserRepository behavior for non-existing user
            _bookRepositoryMock
                .Setup(repo => repo.BookExistsAsync(command.BookId))
                .ReturnsAsync(true);

            _reviewRepositoryMock
                .Setup(repo => repo.GetReviewAsync(command.UserId, command.BookId))
                .ReturnsAsync((Review)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("User never reviewed book", (result as ConflictObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_UpdateBook_ReturnsOkObjectResult()
        {
            // Arrange
            var command = new UpdateReviewCommand
            {
                BookId = 1,
                UserId = 1,
                CreateReviewDTO = new ReviewRetrievalDTO { Rating = Rating.Bad, ReviewContent = " It was a horribly written awkward commedy" }
            };

            var update = new ReviewForUpdateDTO();

            var review = new Review();

            // Set up UserRepository behavior for non-existing user
            _bookRepositoryMock
                .Setup(repo => repo.BookExistsAsync(command.BookId))
                .ReturnsAsync(true);

            _reviewRepositoryMock
                .Setup(repo => repo.GetReviewAsync(command.UserId, command.BookId))
                .ReturnsAsync(review);

            _mapperMock
                .Setup(m => m.Map<ReviewForUpdateDTO>(It.IsAny<ReviewRetrievalDTO>()))
                .Returns(update);

            _mapperMock
                .Setup(m => m.Map(update, review))
                .Returns(review);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfully updated Review", (result as OkObjectResult)?.Value);
        }
    }
}
