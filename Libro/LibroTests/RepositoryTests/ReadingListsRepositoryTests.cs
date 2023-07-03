using Domain.Enums;
using Infrastructure.Repositories;
using Infrastructure;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace LibroTests.RepositoryTests
{
    public class ReadingListsRepositoryTests
    {
        private readonly DbContextOptions<LibroDbContext> options;
        public ReadingListsRepositoryTests()
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
                context.Database.EnsureDeleted();
                context.Dispose();
            }
        }

        [Fact]
        public void GetReadingListOfUser_WithNonExistingUserId_ReturnEmptyList()
        {
            //Arrange

            using (var context = new LibroDbContext(options))
            {
                var _readingListsRepository = new ReadingListsRepository(context);

                //Act
                var result = _readingListsRepository.GetReadingListOfUser(-1);

                //Assert
                Assert.Empty(result);
            }
        }

        [Fact]
        public void GetReadingListOfUser_WithExistingUserId_ReturnListOfReadingLists()
        {
            //Arrange

            using (var context = new LibroDbContext(options))
            {
                var _readingListsRepository = new ReadingListsRepository(context);

                //Act
                var result = _readingListsRepository.GetReadingListOfUser(1);

                //Assert
                Assert.IsType<List<ReadingList>>(result);
            }
        }

        [Fact]
        public async Task ReadingListExistsAsync_WithNonExistingReadingListId_ReturnsFalseResult()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _readingListsRepository = new ReadingListsRepository(context);

                //Act
                var result = await _readingListsRepository.ReadingListExistsAsync(-1);

                //Assert
                Assert.False(result);
            }
        }

        [Fact]
        public async Task ReadingListExistsAsync_WithExistingReadingListId_ReturnsTrueResult()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _readingListsRepository = new ReadingListsRepository(context);

                //Act
                var result = await _readingListsRepository.ReadingListExistsAsync(1);

                //Assert
                Assert.True(result);
            }
        }

        [Fact]
        public async Task AddReadingListAsync_ValidReadingList_ReturnsCompletedResult()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _readingListsRepository = new ReadingListsRepository(context);

                var readingList = new ReadingList { UserId = 1, Title = "Test" };

                //Act
                var result = await _readingListsRepository.AddReadingListAsync(readingList);

                //Assert
                Assert.IsType<Result>(result);
                Assert.Equal(Result.Completed, result);
                Assert.Equal(EntityState.Unchanged, context.Entry(readingList).State);
            }
        }

        [Fact]
        public async Task DeleteReadingListAsync_WithNonExistingReadingListId_ReturnsFailedResult()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _readingListsRepository = new ReadingListsRepository(context);

                //Act
                var result = await _readingListsRepository.DeleteReadingListAsync(-1);

                //Assert
                Assert.Equal(Result.Failed, result);
            }
        }

        [Fact]
        public async Task DeleteReadingListAsync_WithExistingReadingListId_ReturnsCompletedResult()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _readingListsRepository = new ReadingListsRepository(context);

                //Act
                var result = await _readingListsRepository.DeleteReadingListAsync(1);

                //Assert
                Assert.Equal(Result.Completed, result);
            }
        }
    }
}
