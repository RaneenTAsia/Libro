using Application.Entities.ReadingLists.Commands;
using Application.Entities.ReadingLists.Handlers;
using Application.Entities.ReadingLists.Queries;
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

namespace LibroTests.HandlerTests.ReadingListTests
{
    public class GetReadingListHandlerTests
    {
        private readonly Mock<IReadingListsRepository> _readingListsRepositoryMock;
        private readonly Mock<IReadingListItemsFunctionRepository> _readingItemsRepositoryMock;
        private readonly Mock<ILogger<GetReadingListHandler>> _loggerMock;
        private readonly GetReadingListHandler _handler;

        public GetReadingListHandlerTests()
        {
            _loggerMock = new Mock<ILogger<GetReadingListHandler>>();
            _readingItemsRepositoryMock = new Mock<IReadingListItemsFunctionRepository>();
            _readingListsRepositoryMock = new Mock<IReadingListsRepository>();

            _handler = new GetReadingListHandler(
                _readingListsRepositoryMock.Object,
                _readingItemsRepositoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_ReadingListDoesNotExist_ReturnsNotFoundObjectResult()
        {
            // Arrange
            var query = new GetReadingListQuery
            {
                ReadingListId = 1
            };

            _readingListsRepositoryMock
                .Setup(repo => repo.ReadingListExistsAsync(query.ReadingListId))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Item1);
            Assert.Equal("Reading list with this id does not exist", (result.Item1 as NotFoundObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_GetReadingListItems_ReturnsOkObjectResult()
        {
            // Arrange
            var query = new GetReadingListQuery
            {
                ReadingListId = 1
            };

            _readingListsRepositoryMock
                .Setup(repo => repo.ReadingListExistsAsync(query.ReadingListId))
                .ReturnsAsync(true);

            _readingItemsRepositoryMock
                .Setup(repo => repo.GetReadingListAsync(query.ReadingListId))
                .ReturnsAsync(new List<ReadingListItemFunctionResult>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result.Item1);
            Assert.IsType<List<ReadingListItemFunctionResult>>((result.Item1 as OkObjectResult)?.Value);
        }
    }
}
