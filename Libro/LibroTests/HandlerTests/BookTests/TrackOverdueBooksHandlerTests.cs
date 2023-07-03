using Application.DTOs;
using Application.Entities.Books.Handlers;
using Application.Entities.Books.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibroTests.HandlerTests.BookTests
{
    public class TrackOverdueBooksHandlerTests
    {
        private readonly Mock<IViewOverdueBooksDetailsRepository> _viewOverdueBookRepositoryMock;
        private readonly Mock<ILogger<TrackOverdueBooksHandler>> _loggerMock;
        private readonly TrackOverdueBooksHandler _handler;

        public TrackOverdueBooksHandlerTests()
        {
            _loggerMock = new Mock<ILogger<TrackOverdueBooksHandler>>();
            _viewOverdueBookRepositoryMock = new Mock<IViewOverdueBooksDetailsRepository>();

            _handler = new TrackOverdueBooksHandler(
                _viewOverdueBookRepositoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_GetAvailableBooks_ReturnsListofBrowsingBookDto()
        {
            var query = new TrackOverdueBooksQuery();
            // Arrange
            var books = new List<ViewOverdueBookDetails>();
            var browsingBooks = new List<BrowsingBookDTO>();

            _viewOverdueBookRepositoryMock
                .Setup(repo => repo.GetOverdueBooksAsync())
                .ReturnsAsync(books);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsType<List<ViewOverdueBookDetails>>(result.Item1);
        }
    }
}
