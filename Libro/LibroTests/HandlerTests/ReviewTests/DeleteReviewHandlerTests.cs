using Application.DTOs;
using Application.Entities.Reviews.Commands;
using Application.Entities.Reviews.Handlers;
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

namespace LibroTests.HandlerTests.ReviewTests
{
    public class DeleteReviewHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IReviewRepository> _reviewRepositoryMock;
        private readonly Mock<ILogger<DeleteReviewHandler>> _loggerMock;
        private readonly DeleteReviewHandler _handler;

        public DeleteReviewHandlerTests()
        {
            _loggerMock = new Mock<ILogger<DeleteReviewHandler>>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _reviewRepositoryMock = new Mock<IReviewRepository>();

            _handler = new DeleteReviewHandler(
                _bookRepositoryMock.Object,
                _reviewRepositoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_NonExistingbook_ReturnsNotFoundObjectResult()
        {
            // Arrange
            var command = new DeleteReviewCommand
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
        public async Task Handle_NonExistingReview_ReturnsConflictbjectResult()
        {
            // Arrange
            var command = new DeleteReviewCommand
            {
                BookId = 1,
                UserId = 1
            };

            // Set up UserRepository behavior for non-existing user
            _bookRepositoryMock
                .Setup(repo => repo.BookExistsAsync(command.BookId))
                .ReturnsAsync(true);

            _reviewRepositoryMock
                .Setup(repo => repo.ReviewExistsAsync(command.UserId, command.BookId))
                .ReturnsAsync(false);
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("User never reviewed book", (result as ConflictObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_ReviewNotDeleted_ReturnsNotFoundObjectResult()
        {
            // Arrange
            var command = new DeleteReviewCommand
            {
                BookId = 1,
                UserId = 1
            };

            // Set up UserRepository behavior for non-existing user
            _bookRepositoryMock
                .Setup(repo => repo.BookExistsAsync(command.BookId))
                .ReturnsAsync(true);

            _reviewRepositoryMock
                .Setup(repo => repo.ReviewExistsAsync(command.UserId, command.BookId))
                .ReturnsAsync(true);

            _reviewRepositoryMock
                .Setup(repo => repo.DeleteReviewAsync(command.UserId, command.BookId))
                .ReturnsAsync((Review)null);
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("Did not delete Book Review", (result as ConflictObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_ReviewDeleted_ReturnsOkObjectResult()
        {
            // Arrange
            var command = new DeleteReviewCommand
            {
                BookId = 1,
                UserId = 1
            };

            // Set up UserRepository behavior for non-existing user
            _bookRepositoryMock
                .Setup(repo => repo.BookExistsAsync(command.BookId))
                .ReturnsAsync(true);

            _reviewRepositoryMock
                .Setup(repo => repo.ReviewExistsAsync(command.UserId, command.BookId))
                .ReturnsAsync(true);

            _reviewRepositoryMock
                .Setup(repo => repo.DeleteReviewAsync(command.UserId, command.BookId))
                .ReturnsAsync(new Review());
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Successfully Deleted Review", (result as OkObjectResult)?.Value);
        }
    }
}
