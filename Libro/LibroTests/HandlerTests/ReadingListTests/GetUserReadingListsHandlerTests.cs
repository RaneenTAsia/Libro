using Application.DTOs;
using Application.Entities.ReadingLists.Commands;
using Application.Entities.ReadingLists.Handlers;
using Application.Entities.ReadingLists.Queries;
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

namespace LibroTests.HandlerTests.ReadingListTests
{
    public class GetUserReadingListsHandlerTests
    {
        private readonly Mock<IReadingListsRepository> _readingListsRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ILogger<GetUserReadingListsHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetUserReadingListsHandler _handler;

        public GetUserReadingListsHandlerTests()
        {
            _loggerMock = new Mock<ILogger<GetUserReadingListsHandler>>();
            _readingListsRepositoryMock = new Mock<IReadingListsRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _mapperMock = new Mock<IMapper>();

            _handler = new GetUserReadingListsHandler(
                _userRepositoryMock.Object,
                _readingListsRepositoryMock.Object,
                _mapperMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_UserDoesNotExist_ReturnsNotFoundObjectResult()
        {
            // Arrange
            var query = new GetUserReadingListsQuery
            {
                UserId = 1
            };

            _userRepositoryMock
                .Setup(repo => repo.UserExistsByIdAsync(query.UserId))
                .ReturnsAsync(false);
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Item1);
            Assert.Equal("User does not exists", (result.Item1 as NotFoundObjectResult)?.Value);
        }

        [Fact]
        public async Task Handle_GetLists_ReturnsOkObjectResult()
        {
            // Arrange
            var query = new GetUserReadingListsQuery
            {
                UserId = 1
            };

            var readingList = new List<ReadingList>();

            _userRepositoryMock
                .Setup(repo => repo.UserExistsByIdAsync(query.UserId))
                .ReturnsAsync(true);

            _readingListsRepositoryMock
                .Setup(repo => repo.GetReadingListOfUser(query.UserId))
                .Returns(new List<ReadingList>());

            _mapperMock
                .Setup(repo => repo.Map<List<ReadingListDTO>>(It.IsAny<List<ReadingList>>()))
                .Returns(new List<ReadingListDTO>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsType<OkObjectResult>(result.Item1);
            Assert.IsType<List<ReadingListDTO>>((result.Item1 as OkObjectResult)?.Value);
        }
    }
}
