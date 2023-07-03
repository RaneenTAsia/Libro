using Domain.Entities;
using Domain.Enums;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibroTests.RepositoryTests
{
    public class BookTransactionRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<LibroDbContext> options;
        public BookTransactionRepositoryTests()
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
            using(var context = new LibroDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Dispose();
            }
        }

        [Fact]
        public async Task AddBookTranactionAsync_ValidBookTransaction_ReturnsCompletedResult()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _bookTransactionRepository = new BookTransactionRepository(context);
                var transaction= new BookTransaction {BookId = 1, UserId = 1, BorrowDate = DateTime.UtcNow };

                //Act
                var result = await _bookTransactionRepository.AddBookTransactionAsync(transaction);

                //Assert
                Assert.IsType<BookTransaction>(result.Item1);
                Assert.IsType<Result>(result.Item2);
                Assert.Equal(Result.Completed, result.Item2);
                Assert.Equal(EntityState.Unchanged, context.Entry(transaction).State);
            }
        }

        [Fact]
        public async Task BookTransactionById_InValidTransactionId_ReturnsNullResult()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _bookTransactionRepository = new BookTransactionRepository(context);

                //Act
                var result = await _bookTransactionRepository.BookTransactionExistsByIdAsync(-1);

                //Assert
                Assert.IsType<bool>(result);
                Assert.False(result);
            }
        }

        [Fact]
        public async Task BookTransactionById_ValidTransactionId_ReturnsNullResult()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _bookTransactionRepository = new BookTransactionRepository(context);

                //Act
                var result = await _bookTransactionRepository.BookTransactionExistsByIdAsync(1);

                //Assert
                Assert.IsType<bool>(result);
                Assert.True(result);
            }
        }

        [Fact]
        public async Task BookTransactionCurrentCountOfUserByIdAsync_InvalidUserId_Returns0Result()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _bookTransactionRepository = new BookTransactionRepository(context);

                //Act
                var result = await _bookTransactionRepository.BookTransactionCurrentCountOfUserByIdAsync(-1);

                //Assert
                Assert.Equal(0, result);
            }
        }

        [Fact]
        public async Task BookTransactionCurrentCountOfUserByIdAsync_ValidUserId_Returns3Result()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _bookTransactionRepository = new BookTransactionRepository(context);

                //Act
                var result = await _bookTransactionRepository.BookTransactionCurrentCountOfUserByIdAsync(3);

                //Assert
                Assert.NotEqual(1, result);
            }
        }

        [Fact]
        public async Task OngoingBookTransactionByIdAsync_InvalidUserId_ReturnsNullResult()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _bookTransactionRepository = new BookTransactionRepository(context);

                //Act
                var result = await _bookTransactionRepository.OngoingBookTransationByBookIdAsync(-1);

                //Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task OngoingBookTransactionByIdAsync_ValidUserId_ReturnsBookTransactionResult()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _bookTransactionRepository = new BookTransactionRepository(context);

                //Act
                var result = await _bookTransactionRepository.OngoingBookTransationByBookIdAsync(2);

                //Assert
                Assert.IsType<BookTransaction>(result);
                Assert.Equal(2, result.BookId);
            }
        }

        [Fact]
        public async Task GetOverdueBookBookTransactionAsync_ReturnsListOfBookTrandactions()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _bookTransactionRepository = new BookTransactionRepository(context);



                //Act
                var result = await _bookTransactionRepository.GetOverdueBookTransactionsAsync();

                //Assert
                Assert.IsType<List<BookTransaction>>(result);
            }
        }


        [Fact]
        public async Task GetUserBorrowingHistoryAsync_InvalidUserId_ReturnsEmptyListResult()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _bookTransactionRepository = new BookTransactionRepository(context);

                //Act
                var result = await _bookTransactionRepository.GetUserBorrowingHistoryAsync(-1);

                //Assert
                Assert.Empty(result);
            }
        }

        [Fact]
        public async Task GetUserBorrowingHistoryAsync_ValidUserId_ReturnsListOfBookTransactionResult()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _bookTransactionRepository = new BookTransactionRepository(context);

                //Act
                var result = await _bookTransactionRepository.GetUserBorrowingHistoryAsync(2);

                //Assert
                Assert.IsType<List<BookTransaction>>(result);
            }
        }
    }
}
