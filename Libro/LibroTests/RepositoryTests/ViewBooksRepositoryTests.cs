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
    public class ViewBooksRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<LibroDbContext> options;
        public ViewBooksRepositoryTests()
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
        public async Task GetBooksAsync_ReturnsListOfViewBooks()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _viewBooksRepository = new ViewBooksRepository(context);

                //Act
                var books = await _viewBooksRepository.GetBooksAsync();

                //Assert
                Assert.IsType<List<ViewBooks>>(books);
            }
        }

        [Fact]
        public void GetBooksWithAuthorAsync_ValidAuthor_ReturnsListOfViewBooksWithAuthor()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _viewBooksRepository = new ViewBooksRepository(context);

                //Act
                var books =_viewBooksRepository.GetBooksWithAuthor("J.K.Rowling");

                //Assert
                Assert.IsType<List<ViewBooks>>(books);
                foreach(var book in books)
                {
                    Assert.Contains(book.Authors, "J.K.Rowling");
                }
            }
        }

        [Fact]
        public void GetBooksWithAuthorAsync_InvalidAuthor_ReturnsEmptyList()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _viewBooksRepository = new ViewBooksRepository(context);

                //Act
                var books = _viewBooksRepository.GetBooksWithAuthor("Test");

                //Assert
                Assert.Empty(books);
            }
        }

        [Fact]
        public void GetBooksWithTitleAsync_InvalidTitle_ReturnsEmptyList()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _viewBooksRepository = new ViewBooksRepository(context);

                //Act
                var books = _viewBooksRepository.GetBooksWithTitle("Title");

                //Assert
                Assert.Empty(books);
            }
        }

        [Fact]
        public void GetBooksWithTitleAsync_ValidTitle_ReturnsListOfViewBooksWithTitle()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _viewBooksRepository = new ViewBooksRepository(context);

                //Act
                var books = _viewBooksRepository.GetBooksWithTitle("IT");

                //Assert
                Assert.IsType<List<ViewBooks>>(books);
                foreach (var book in books)
                {
                    Assert.Contains(book.Title, "IT");
                }
            }
        }

        [Fact]
        public void GetBooksWithIds_WithNonExistingBookIds_ReturnsEmptyList()
        {
            //Arrange
            var bookIds = new List<int>
            {
                -1
            };

            using (var context = new LibroDbContext(options))
            {

                var _viewBooksRepository = new ViewBooksRepository(context);

                //Act
                var books = _viewBooksRepository.GetBooksWithIds(bookIds);

                //Assert
                Assert.IsType<List<ViewBooks>>(books);
                Assert.Empty(books);
            }
        }

        [Fact]
        public void GetBooksWithIdsAsync_WithExistingBookIds_ReturnsListOfViewBooks()
        {
            //Arrange
            var bookIds = new List<int>
            {
                1, 2, 3
            };

            using (var context = new LibroDbContext(options))
            {

                var _viewBooksRepository = new ViewBooksRepository(context);

                //Act
                var books = _viewBooksRepository.GetBooksWithIds(bookIds);

                //Assert
                Assert.IsType<List<ViewBooks>>(books);
                Assert.Equal(3, books.Count);
                foreach (var bookId in bookIds)
                {
                    Assert.Contains(books, book => book.BookId == bookId);
                }
            }
        }

    }
}
