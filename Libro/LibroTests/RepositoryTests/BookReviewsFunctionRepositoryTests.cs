using Domain.Entities;
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
    public class BookReviewsFunctionRepositoryTests: IDisposable
    {
        private readonly DbContextOptions<LibroDbContext> options;
        public BookReviewsFunctionRepositoryTests()
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
        public async Task GetBookReviewsAsync_GetReservations_ReturnsListOfBookFunctionResult()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _bookReviewsFunctionRepository = new BookReviewsFunctionRepository(context);

                //Act
                var bookReviews = await _bookReviewsFunctionRepository.GetBookReviewsAsync(1);

                //Assert
                Assert.IsType<List<BookReviewsFunctionResult>>(bookReviews);
            }
        }
    }
}
