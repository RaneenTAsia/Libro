using Infrastructure.Repositories;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enums;

namespace LibroTests.RepositoryTests
{
    public class BookReservationRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<LibroDbContext> options;
        public BookReservationRepositoryTests()
        {
            options = new DbContextOptionsBuilder<LibroDbContext>()
               .UseSqlServer("Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = LibroTest" + Guid.NewGuid().ToString())
               .Options;

            using (var context = new LibroDbContext(options))
            {
                context.Database.Migrate();
            }
        }

        public void Dispose()
        {
            using (var context = new LibroDbContext(options))
            {
                context.Dispose();
            }
        }

        [Fact]
        public async Task AddBookReservationAsync_ValidBookReservation_ReturnsCompletedResult()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _bookReservationRepository = new BookReservationRepository(context);
                var bookReservation = new BookReservation { BookId = 1, UserId = 1, ReserveDate = DateTime.UtcNow };

                //Act
                var result = await _bookReservationRepository.AddBookReservationAsync(bookReservation);

                //Assert
                Assert.IsType<Result>(result);
                Assert.Equal(Result.Completed, result);
                context.Dispose();
            }
        }

        [Theory]
        [InlineData(1,-1)]
        [InlineData(-1,-1)]
        [InlineData(-1, 1)]
        public async Task BookReservationExistsAsync_WithNonExistingUserOrBookId_ReturnsFalseResult(int userId, int bookId)
        {
            //Arrange

            using (var context = new LibroDbContext(options))
            {
                var _bookReservationRepository = new BookReservationRepository(context);

                //Act
                var result = await _bookReservationRepository.BookReservationExistsAsync(userId, bookId);

                //Assert
                Assert.False(result);
            }
        }

        [Fact]
        public async Task BookReservationExistsAsync_WithValidIds_ReturnsTrueResult()
        {
            //Arrange

            using (var context = new LibroDbContext(options))
            {
                var _bookReservationRepository = new BookReservationRepository(context);

                //Act
                var result = await _bookReservationRepository.BookReservationExistsAsync(4, 1);

                //Assert
                Assert.True(result);
            }
        }

        [Fact]
        public async Task DeleteBookReservationAsync_WithValidBookReservation_ReturnsCompletedResult()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _bookReservationRepository = new BookReservationRepository(context);
                var bookReservation = new BookReservation { BookId = 1, UserId = 4, BookReservationId = 1};
                //Act
                var result = await _bookReservationRepository.DeleteBookReservationAsync(bookReservation);

                //Assert
                Assert.Equal(Result.Completed, result);
                Assert.Equal(EntityState.Detached, context.Entry(bookReservation).State);
            }
        }

        [Theory]
        [InlineData(1,-1)]
        [InlineData(-1,-1)]
        [InlineData(-1,1)]
        public void GetBookReservationByIdAsync_WithNonExistingBookId_ReturnsNull(int bookId, int userId)
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _bookReservationRepository = new BookReservationRepository(context);

                //Act
                var bookReservation =_bookReservationRepository.GetBookReservation(userId, bookId);

                //Assert
                Assert.Null(bookReservation);
            }
        }

        [Fact]
        public void GetBookReservationByIdAsync_WithExistingBookAndUserIds_ReturnsBookReservation()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _bookReservationRepository = new BookReservationRepository(context);

                //Act
                var bookReservation = _bookReservationRepository.GetBookReservation(4, 1);

                //Assert
                Assert.IsType<BookReservation?>(bookReservation);
                Assert.Equal(1, bookReservation.BookId);
                Assert.Equal(4, bookReservation.UserId);
            }
        }

        [Fact]
        public async Task RemoveBookReservationsAsync_GetReservations_ResturnsListOfRemovedReservations()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _bookReservationRepository = new BookReservationRepository(context);

                //Act
                var bookReservations = await _bookReservationRepository.RemoveBookReservationsAsync();

                //Assert
                Assert.IsType<List<BookReservation>>(bookReservations);
                foreach(var reservation in bookReservations)
                Assert.Equal(EntityState.Detached, context.Entry(reservation).State);
            }
        }
    }
}
