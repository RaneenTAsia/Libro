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
    public class ReviewRepositoryTests
    {
        private readonly DbContextOptions<LibroDbContext> options;
        public ReviewRepositoryTests()
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

        [Theory]
        [InlineData(1, -1)]
        [InlineData(-1, -1)]
        [InlineData(-1, 1)]
        public async Task ReviewExistsAsync_WithNonExistingUserIdOrBookId_ReturnsFalse(int userId, int bookId)
        {
            //Arrange

            using (var context = new LibroDbContext(options))
            {
                var _reviewRepository = new ReviewRepository(context);

                //Act
                var result = await  _reviewRepository.ReviewExistsAsync(userId, bookId);

                //Assert
                Assert.False(result);
            }
        }

        [Fact]
        public async Task ReviewExistsAsync_WithExistingUserIdOrBookId_ReturnsTrue()
        {
            //Arrange

            using (var context = new LibroDbContext(options))
            {
                var _reviewRepository = new ReviewRepository(context);

                //Act
                var result = await _reviewRepository.ReviewExistsAsync(4,3);

                //Assert
                Assert.True(result);
            }
        }

        [Theory]
        [InlineData(1, -1)]
        [InlineData(-1, -1)]
        [InlineData(-1, 1)]
        public async Task GetReviewAsync_WithNonExistingUserIdOrBookId_ReturnsNull(int userId, int bookId)
        {
            //Arrange

            using (var context = new LibroDbContext(options))
            {
                var _reviewRepository = new ReviewRepository(context);

                //Act
                var result = await _reviewRepository.GetReviewAsync(userId, bookId);

                //Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task GetReviewAsync_WithExistingUserIdAndBookId_ReturnsReview()
        {
            //Arrange

            using (var context = new LibroDbContext(options))
            {
                var _reviewRepository = new ReviewRepository(context);

                //Act
                var result = await _reviewRepository.GetReviewAsync(4,3);

                //Assert
                Assert.IsType<Review>(result);
            }
        }

        [Fact]
        public async Task CreateReviewAsync_ValidReview_ReturnsCompletedResult()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _reviewRepository = new ReviewRepository(context);

                var review = new Review { UserId = 1, BookId = 4, Rating = Rating.Ok, ReviewContent = "Test" };

                //Act
                var result = await _reviewRepository.CreateReviewAsync(review);

                //Assert
                Assert.IsType<Result>(result);
                Assert.Equal(Result.Completed, result);
                Assert.Equal(EntityState.Unchanged, context.Entry(review).State);
            }
        }

        [Theory]
        [InlineData(1, -1)]
        [InlineData(-1, -1)]
        [InlineData(-1, 1)]
        public async Task DeleteReviewAsync_WithNonExistingUserIdAndBookId_ReturnsNull(int userId, int bookId)
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _reviewRepository = new ReviewRepository(context);

                //Act
                var result = await _reviewRepository.DeleteReviewAsync(userId, bookId);

                //Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task DeleteReviewAsync_WithExistingUserIdAndBookId_ReturnsReview()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _reviewRepository = new ReviewRepository(context);

                //Act
                var result = await _reviewRepository.DeleteReviewAsync(4, 3);

                //Assert
                Assert.IsType<Review>(result);
            }
        }
    }
}
