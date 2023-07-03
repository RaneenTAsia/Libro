using Domain.Entities;
using Domain.Enums;
using Infrastructure.Repositories;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace LibroTests.RepositoryTests
{
    public class ReadingItemsRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<LibroDbContext> options;
        public ReadingItemsRepositoryTests()
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
            using( var context = new LibroDbContext(options) )
            {
                context.Database.EnsureDeleted();
                context.Dispose();
            }
        }

        [Theory]
        [InlineData(1, -1)]
        [InlineData(-1, -1)]
        [InlineData(-1, 1)]
        public async Task BookExistsInListAsync_WithNonExistingBookIdOrEReadingListId_ReturnsFalseResult(int bookId, int readingListId)
        {
            //Arrange

            using (var context = new LibroDbContext(options))
            {
                var _readingItemsRepository = new ReadingItemsRepository(context);

                //Act
                var result = await _readingItemsRepository.BookExistsInListAsync(bookId, readingListId);

                //Assert
                Assert.False(result);
            }
        }

        [Fact]
        public async Task BookExistsAsync_WithExistingBookId_ReturnsTrueResult()
        {
            //Arrange

            using (var context = new LibroDbContext(options))
            {
                var _readingItemsRepository = new ReadingItemsRepository(context);

                //Act
                var result = await _readingItemsRepository.BookExistsInListAsync(1,1);

                //Assert
                Assert.True(result);
            }
        }

        [Fact]
        public async Task AddBookToReadingListAsync_ValidBookIdAndReadingLIstId_ReturnsCompletedResult()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _readingItemsRepository = new ReadingItemsRepository(context);

                //Act
                var result = await _readingItemsRepository.AddBookToReadingList(1,1);

                //Assert
                Assert.IsType<Result>(result);
                Assert.Equal(Result.Completed, result);
            }
        }

        [Theory]
        [InlineData(1, -1)]
        [InlineData(-1, -1)]
        [InlineData(-1, 1)]
        public async Task DeleteBookFromReadingListAsync_WithNonExistingBookIdOrReadingListId_ReturnsFaiiledResult(int bookId, int readingListId)
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _readingItemsRepository = new ReadingItemsRepository(context);

                //Act
                var result = await _readingItemsRepository.DeleteBookFromReadingListAsync(bookId, readingListId);

                //Assert
                Assert.Equal(Result.Failed, result);
            }
        }

        [Fact]
        public async Task DeleteBookAsync_WithExistingBookId_ReturnsFailedResult()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _readingItemsRepository = new ReadingItemsRepository(context);

                //Act
                var result = await _readingItemsRepository.DeleteBookFromReadingListAsync(1, 1);
                //Assert
                Assert.Equal(Result.Completed, result);
            }
        }
    }
}
