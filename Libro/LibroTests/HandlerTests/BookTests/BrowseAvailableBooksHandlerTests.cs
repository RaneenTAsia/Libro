using Application.DTOs;
using Application.Entities.Books.Handlers;
using Application.Entities.Books.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace LibroTests.HandlerTests.BookTests
{
    public class BrowseAvailableBooksHandlerTests
    {
        private readonly Mock<IViewBooksRepository> _viewBookRepositoryMock;
        private readonly Mock<ILogger<BrowseAvailableBooksHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly BrowseAvailableBooksHandler _handler;

        public BrowseAvailableBooksHandlerTests()
        {
            _loggerMock = new Mock<ILogger<BrowseAvailableBooksHandler>>();
            _mapperMock = new Mock<IMapper>();
            _viewBookRepositoryMock = new Mock<IViewBooksRepository>();

            _handler = new BrowseAvailableBooksHandler(
                _viewBookRepositoryMock.Object,
                _loggerMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task Handle_GetAvailableBooks_ReturnsListofBrowsingBookDto()
        {
            var query = new BrowseAvailableBooksQuery();
            // Arrange
            var books = new List<ViewBooks>();
            var browsingBooks = new List<BrowsingBookDTO>();

            _viewBookRepositoryMock
                .Setup(m => m.GetBooksAsync())
                .ReturnsAsync(books);

            _mapperMock
                .Setup(m => m.Map<List<BrowsingBookDTO>>(books))
                .Returns(browsingBooks);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsType<List<BrowsingBookDTO>>(result.Item1);
        }
    }
}
