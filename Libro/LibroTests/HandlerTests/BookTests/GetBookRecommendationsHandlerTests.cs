using Application.DTOs;
using Application.Entities.Books.Handlers;
using Application.Entities.Books.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Repositories;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LibroTests.HandlerTests.BookTests
{
    public class GetBookRecommendationsHandlerTests
    {
        private readonly Mock<IViewBooksRepository> _viewBookRepositoryMock;
        private readonly Mock<IBookToGenreRepository> _bookToGenreRepositoryMock;
        private readonly Mock<IBookTransactionRepository> _bookTransactionRepositoryMock;
        private readonly Mock<ILogger<GetBookRecommendationsHandler>> _loggerMock;
        private readonly GetBookRecommendationsHandler _handler;

        public GetBookRecommendationsHandlerTests()
        {
            _loggerMock = new Mock<ILogger<GetBookRecommendationsHandler>>();
            _viewBookRepositoryMock = new Mock<IViewBooksRepository>();
            _bookToGenreRepositoryMock = new Mock<IBookToGenreRepository>();
            _bookTransactionRepositoryMock = new Mock<IBookTransactionRepository>();

            _handler = new GetBookRecommendationsHandler(
                _bookTransactionRepositoryMock.Object,
                _bookToGenreRepositoryMock.Object,
                _viewBookRepositoryMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_GetBookDetails_ReturnsBookDetails()
        {
            var query = new GetBookRecommendationsQuery
            {
                UserId = 1
            };
            // Arrange
            var books = new List<ViewBooks>
            {
                new ViewBooks {BookId = 1, Title = "Test" }
            };

            var history = new List<BookTransaction>
            {
                new BookTransaction{ UserId = 1, BookId = 1, BookTransactionId = 1}
            };

            var foundBookIds = new List<BookToBookGenre>
            {
                new BookToBookGenre{ BookId = 1, BookGenreId = Genre.Mystery }
            };

            _bookTransactionRepositoryMock
                .Setup(repo => repo.GetUserBorrowingHistoryAsync(query.UserId))
                .ReturnsAsync(history);

            _bookToGenreRepositoryMock
                .Setup(repo => repo.GetTop2GenresOfBooks(It.IsAny<List<int>>()))
                .Returns(new List<Genre> { Genre.Romance, Genre.Mystery });

            _bookToGenreRepositoryMock
                .Setup(repo => repo.GetBookIdsByGenreId(It.IsAny<int>()))
                .Returns(foundBookIds);

            _viewBookRepositoryMock
                .Setup(m => m.GetBooksWithIds(It.IsAny<List<int>>()))
                .Returns(books);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsType<List<ViewBooks>>(result.Item1);
        }
    }
}
