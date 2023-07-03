using Domain.Entities;
using Domain.Enums;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibroTests.RepositoryTests
{
    public class AuthorRepositoryTests: IDisposable
    {
        private readonly DbContextOptions<LibroDbContext> options;

        public AuthorRepositoryTests()
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
        public async Task GetAuthorsByIdsAsync_WithExistingAuthorIds_ReturnsListOfAuthors()
        {
            //Arrange
            var authorIds = new List<int>
            {
                1, 2, 3
            };

            using (var context = new LibroDbContext(options))
            {
                var _authorRepository = new AuthorRepository(context);

                //Act
                var authors = await _authorRepository.GetAuthorsByIdsAsync(authorIds);

                //Assert
                Assert.IsType<List<Author>>(authors);
                Assert.Equal(3, authors.Count);
                foreach (var authorId in authorIds)
                {
                    Assert.Contains(authors, author => author.AuthorId == authorId);
                }
            }
        }

        [Fact]
        public async Task GetAuthorsByIdsAsync_WithNonExistingAuthorIds_ReturnsEmptyListOfAuthors()
        {
            //Arrange
            var authorIds = new List<int>
            {
                -3
            };

            using (var context = new LibroDbContext(options))
            {
                var _authorRepository = new AuthorRepository(context);

                //Act
                var authors = await _authorRepository.GetAuthorsByIdsAsync(authorIds);

                //Assert
                Assert.IsType<List<Author>>(authors);
                Assert.Empty(authors);
            }
        }

        [Fact]
        public async Task AuthorExitsAsync_AuthorDoesntExist_Returnsfalse()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _authorRepository = new AuthorRepository(context);

                //Act
                var result = await _authorRepository.AuthorExistsAsync(-1);

                //Assert
                Assert.IsType<bool>(result);
                Assert.False(result);
            }
        }

        [Fact]
        public async Task AuthorExitsAsync_AuthorExists_Returnstrue()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _authorRepository = new AuthorRepository(context);

                //Act
                var result = await _authorRepository.AuthorExistsAsync(1);

                //Assert
                Assert.IsType<bool>(result);
                Assert.True(result);
            }
        }

        [Fact]
        public async Task AddAuthorAsync_ValidAuthor_ReturnsCompletedResult()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _authorRepository = new AuthorRepository(context);
                var author = new Author { Description = "Test", Name = "Test" };

                //Act
                var result = await _authorRepository.AddAuthorAsync(author);

                //Assert
                Assert.IsType<Result>(result);
                Assert.Equal(Result.Completed, result);
                Assert.Equal(EntityState.Unchanged, context.Entry(author).State);
            }
        }

        [Fact]
        public async Task GetAuthorByIdAsync_WithExistingAuthorId_ReturnsAuthor()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _authorRepository = new AuthorRepository(context);

                //Act
                var author = await _authorRepository.GetAuthorByIdAsync(1);

                //Assert
                Assert.IsType<Author>(author);

                Assert.Equal(1, author.AuthorId);
            }
        }

        [Fact]
        public async Task GetAuthorByIdAsync_WithNonExistingAuthorId_ReturnsNull()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _authorRepository = new AuthorRepository(context);

                //Act
                var author = await _authorRepository.GetAuthorByIdAsync(-1);

                //Assert
                Assert.Null(author);
            }
        }

        [Fact]
        public async Task DeleteAuthorAsync_WithNonExistingAuthorId_ReturnsNull()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _authorRepository = new AuthorRepository(context);

                //Act
                var author = await _authorRepository.DeleteAuthorAsync(-1);

                //Assert
                Assert.Null(author);
            }
        }

        [Fact]
        public async Task DeleteAuthorAsync_WithExistingAuthorId_ReturnsAuthor()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _authorRepository = new AuthorRepository(context);

                //Act
                var author = await _authorRepository.DeleteAuthorAsync(1);

                //Assert
                Assert.IsType<Author>(author);
                Assert.Equal(EntityState.Deleted, context.Entry(author).State);
            }
        }
    }
}
