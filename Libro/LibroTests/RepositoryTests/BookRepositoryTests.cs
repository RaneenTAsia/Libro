using Domain.Entities;
using Infrastructure.Repositories;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace LibroTests.RepositoryTests
{
    public class BookRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<LibroDbContext> options;
        public BookRepositoryTests()
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
        public async Task GetBookByIdAsync_WithNonExistingBookId_ReturnsNull()
        {
            //Arrange

            using (var context = new LibroDbContext(options))
            {
                var _bookRepository = new BookRepository(context);

                //Act
                var book = await _bookRepository.GetBookByIdAsync(-1);

                //Assert
                Assert.Null(book);
            }
        }

        [Fact]
        public async Task GetBookByIdAsync_WithExistingBookId_ReturnsBook()
        {
            //Arrange

            using (var context = new LibroDbContext(options))
            {
                var _bookRepository = new BookRepository(context);

                //Act
                var book = await _bookRepository.GetBookByIdAsync(2);

                //Assert
                Assert.IsType<Book>(book);
                Assert.True(book.BookId == 2);
            }
        }

        [Fact]
        public async Task SetBookAsReservedAsync_WithNonExistingBookId_ReturnsFailedResult()
        {
            //Arrange

            using (var context = new LibroDbContext(options))
            {
                var _bookRepository = new BookRepository(context);

                //Act
                var result = await _bookRepository.SetBookAsReservedAsync(-1);

                //Assert
                Assert.Equal(Result.Failed, result);
            }
        }


        [Fact]
        public async Task SetBookAsReservedAsync_WithExistingBookId_ReturnsCompletedResult()
        {
            //Arrange

            using (var context = new LibroDbContext(options))
            {
                var _bookRepository = new BookRepository(context);

                //Act
                var result = await _bookRepository.SetBookAsReservedAsync(4);

                //Assert
                Assert.Equal(Result.Completed, result);
            }
        }

        [Fact]
        public async Task CheckBookIsAvailableAsync_WithExistingAvailableBookId_ReturnsTrueResult()
        {
            //Arrange

            using (var context = new LibroDbContext(options))
            {
                var _bookRepository = new BookRepository(context);

                //Act
                var result = await _bookRepository.CheckBookIsAvailableAsync(4);

                //Assert
                Assert.True(result);
            }
        }

        [Fact]
        public async Task CheckBookIsAvailableAsync_WithExistingReservedBookId_ReturnsFalseResult()
        {
            //Arrange

            using (var context = new LibroDbContext(options))
            {
                var _bookRepository = new BookRepository(context);

                //Act
                var result = await _bookRepository.CheckBookIsAvailableAsync(2);

                //Assert
                Assert.False(result);
            }
        }

        [Fact]
        public async Task GetBookStatusByIdAsync_WithExistingBookId_ReturnsStatusResult()
        {
            //Arrange

            using (var context = new LibroDbContext(options))
            {
                var _bookRepository = new BookRepository(context);

                //Act
                var result = await _bookRepository.GetBookStatusByIdAsync(2);

                //Assert
                Assert.IsType<Status>(result);
            }
        }

        [Fact]
        public async Task BookExistsAsync_WithNonExistingBookId_ReturnsFalseResult()
        {
            //Arrange

            using (var context = new LibroDbContext(options))
            {
                var _bookRepository = new BookRepository(context);

                //Act
                var result = await _bookRepository.BookExistsAsync(-1);

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
                var _bookRepository = new BookRepository(context);

                //Act
                var result = await _bookRepository.BookExistsAsync(2);

                //Assert
                Assert.True(result);
            }
        }

        [Fact]
        public async Task GetBooksByIdsAsync_WithExistingBookIds_ReturnsListOfBooks()
        {
            //Arrange
            var bookIds = new List<int>
            {
                2, 3
            };

            using (var context = new LibroDbContext(options))
            {
                var _bookRepository = new BookRepository(context);

                //Act
                var books = await _bookRepository.GetBooksByIdsAsync(bookIds);

                //Assert
                Assert.IsType<List<Book>>(books);
                Assert.Equal(2, books.Count);
                foreach (var bookId in bookIds)
                {
                    Assert.Contains(books, book => book.BookId == bookId);
                }
            }
        }

        [Fact]
        public async Task GetBooksByIdsAsync_WithNonExistingBookIds_ReturnsEmptyListOfBooks()
        {
            //Arrange
            var bookIds = new List<int>
            {
                -1
            };

            using (var context = new LibroDbContext(options))
            {
                var _bookRepository = new BookRepository(context);

                //Act
                var books = await _bookRepository.GetBooksByIdsAsync(bookIds);

                //Assert
                Assert.IsType<List<Book>>(books);
                Assert.Empty(books);
            }
        }

        [Fact]
        public async Task AddBookAsync_ValidBook_ReturnsCompletedResult()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _bookRepository = new BookRepository(context);
                var book = new Book { Description = "Test", Title = "Test",BookStatus = 1 };

                //Act
                var result = await _bookRepository.AddBookAsync(book);

                //Assert
                Assert.IsType<Result>(result);
                Assert.Equal(Result.Completed, result);
                Assert.Equal(EntityState.Unchanged, context.Entry(book).State);
            }
        }

        [Fact]
        public async Task DeleteBookAsync_WithNonExistingBookId_ReturnsFailedResult()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _bookRepository = new BookRepository(context);

                //Act
                var result = await _bookRepository.DeleteBookAsync(-1);

                //Assert
                Assert.Equal(Result.Failed, result);
            }
        }

        //[Fact]
        //public async Task DeleteBookAsync_WithExistingBookId_ReturnsCompletedResult()
        //{
        //    //Arrange
        //    using (var context = new LibroDbContext(options))
        //    {
        //        var _bookRepository = new BookRepository(context);

        //        //Act
        //        var result = await _bookRepository.DeleteBookAsync(1);

        //        //Assert
        //        Assert.Equal(Result.Completed, result);
        //    }
        //}
    }
}
