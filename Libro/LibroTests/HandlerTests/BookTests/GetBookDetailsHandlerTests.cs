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
    public class GetBookDetailsHandlerTests
    {
        private readonly Mock<IViewBooksRepository> _viewBookRepositoryMock;
        private readonly Mock<ILogger<GetBookDetailsHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetBookDetailsHandler _handler;

        public GetBookDetailsHandlerTests()
        {
            _loggerMock = new Mock<ILogger<GetBookDetailsHandler>>();
            _mapperMock = new Mock<IMapper>();
            _viewBookRepositoryMock = new Mock<IViewBooksRepository>();

            _handler = new GetBookDetailsHandler(
                _viewBookRepositoryMock.Object,
                _loggerMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task Handle_GetBookDetails_ReturnsBookDetails()
        {
            var query = new GetBookDetailsQuery
            {
                BookId = 1
            };
            // Arrange
            var books = new List<ViewBooks>
            {
                new ViewBooks {BookId = 1, Title = "Test"}
            };

            var bookDetailsDto = new BookDetailsDTO
            {
                Title = "Test"
            };

            _viewBookRepositoryMock
                .Setup(m => m.GetBooksAsync())
                .ReturnsAsync(books);

            _mapperMock
                .Setup(m => m.Map<BookDetailsDTO>(It.IsAny<ViewBooks>()))
                .Returns(bookDetailsDto);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsType<BookDetailsDTO>(result);
        }
    }
}
