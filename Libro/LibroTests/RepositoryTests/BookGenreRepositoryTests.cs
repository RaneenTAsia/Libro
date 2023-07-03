using Domain.Entities;
using Infrastructure.Repositories;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibroTests.RepositoryTests
{
    public class BookGenreRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<LibroDbContext> options;
        public BookGenreRepositoryTests()
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
        public async Task GetGenresByIdsAsync_WithExistingGenreIds_ReturnsListOfBookGenres()
        {
            //Arrange
            var genreIds = new List<int>
            {
                1, 2, 3
            };

            using (var context = new LibroDbContext(options))
            {
                var _bookGenreRepository = new BookGenreRepository(context);

                //Act
                var genres = await _bookGenreRepository.GetGenresByIdsAsync(genreIds);

                //Assert
                Assert.IsType<List<BookGenre>>(genres);
                Assert.Equal(3, genres.Count);
                foreach (var genreId in genreIds)
                {
                    Assert.Contains(genres, genre => (int)genre.BookGenreId == genreId);
                }
            }
        }

        [Fact]
        public async Task GetGenresByIdsAsync_WithNonExistingGenreIds_ReturnsEmptyListOfGenres()
        {
            //Arrange
            var genreIds = new List<int>
            {
                -3
            };

            using (var context = new LibroDbContext(options))
            {
                var _bookGenreRepository = new BookGenreRepository(context);

                //Act
                var genres = await _bookGenreRepository.GetGenresByIdsAsync(genreIds);

                //Assert
                Assert.IsType<List<BookGenre>>(genres);
                Assert.Empty(genres);
            }
        }
    }
}
