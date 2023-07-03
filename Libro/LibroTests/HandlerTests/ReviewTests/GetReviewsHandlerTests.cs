using Application.Entities.Reviews.Commands;
using Application.Entities.Reviews.Handlers;
using Application.Entities.Reviews.Queries;
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
    public class GetReviewsHandlerTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IBookReviewsFunctionRepository> _bookRevieweFunctionRepositoryMock;
        private readonly Mock<ILogger<GetReviewsHandler>> _loggerMock;
        private readonly GetReviewsHandler _handler;

        public GetReviewsHandlerTests()
        {
            _loggerMock = new Mock<ILogger<GetReviewsHandler>>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            _bookRevieweFunctionRepositoryMock = new Mock<IBookReviewsFunctionRepository>();

            _handler = new GetReviewsHandler(
                _bookRepositoryMock.Object,
                _bookRevieweFunctionRepositoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_NonExistingbook_ReturnsNotFoundObjectResult()
        {
            // Arrange
            var query = new GetReviewsQuery
            {
                BookId = 1
            };

            // Set up UserRepository behavior for non-existing user
            _bookRepositoryMock
                .Setup(repo => repo.BookExistsAsync(query.BookId))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Item1);
            Assert.Equal("Book does not exist", (result.Item1 as NotFoundObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_GetReviews_ReturnsOkObjectResult()
        {
            // Arrange
            var query = new GetReviewsQuery
            {
                BookId = 1,
                pageNumber = 1,
                pageSize = 1
            };

            // Set up UserRepository behavior for non-existing user
            _bookRepositoryMock
                .Setup(repo => repo.BookExistsAsync(query.BookId))
                .ReturnsAsync(true);

            _bookRevieweFunctionRepositoryMock
                .Setup(repo => repo.GetBookReviewsAsync(query.BookId))
                .ReturnsAsync(new List<BookReviewsFunctionResult>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result.Item1);
            Assert.IsType<List<BookReviewsFunctionResult>>((result.Item1 as OkObjectResult)?.Value);
        }
    }
}
