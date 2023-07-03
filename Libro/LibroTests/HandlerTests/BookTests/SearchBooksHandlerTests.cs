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
    public class SearchBooksHandlerTests
    {
        private readonly Mock<IViewBooksRepository> _viewBookRepositoryMock;
        private readonly Mock<IBookToGenreRepository> _bookToGenreRepositoryMock;
        private readonly Mock<ILogger<SearchBooksHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly SearchBooksHandler _handler;

        public SearchBooksHandlerTests()
        {
            _loggerMock = new Mock<ILogger<SearchBooksHandler>>();
            _mapperMock = new Mock<IMapper>();
            _viewBookRepositoryMock = new Mock<IViewBooksRepository>();
            _bookToGenreRepositoryMock = new Mock<IBookToGenreRepository>();

            _handler = new SearchBooksHandler(
                _viewBookRepositoryMock.Object,
                _bookToGenreRepositoryMock.Object,
                _loggerMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task Handle_GetAvailableBooks_ReturnsListofBrowsingBookDto()
        {
            var query = new SearchBooksQuery();
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
