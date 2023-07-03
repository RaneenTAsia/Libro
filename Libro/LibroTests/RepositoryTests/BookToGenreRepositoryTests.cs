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
    public class BookToGenreRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<LibroDbContext> options;
        public BookToGenreRepositoryTests()
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
        public void GetBookIdsByGenreId_GetBookToBookGenresOfExistingBookId_ReturnsListOfBookToBookGenres()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _bookToAuthorRepository = new BookToGenreRepository(context);

                //Act
                var bookToBookGenres = _bookToAuthorRepository.GetBookIdsByGenreId(1);

                //Assert
                Assert.IsType<List<BookToBookGenre>>(bookToBookGenres);
                foreach (var bookToGenre in bookToBookGenres)
                    Assert.Equal(1, (int)bookToGenre.BookGenreId);
            }
        }

        [Fact]
        public void GetBookIdsByGenreId_GetBookToBookGenresOfNonExistingBookId_ResturnsListOfBookToBookGenres()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _bookToAuthorRepository = new BookToGenreRepository(context);

                //Act
                var bookToBookGenres = _bookToAuthorRepository.GetBookIdsByGenreId(-1);

                //Assert
                Assert.Empty(bookToBookGenres);
            }
        }


        [Fact]
        public void GetTop2GenresOfBooks__ReturnsListOf2Genres()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _bookToAuthorRepository = new BookToGenreRepository(context);
                var list = new List<int> { 1, 2, 3 };

                //Act
                var genres = _bookToAuthorRepository.GetTop2GenresOfBooks(list);

                //Assert
                Assert.Equal(2, genres.Count());
            }
        }
    }
}
