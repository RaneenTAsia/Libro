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

namespace LibroTests.RepositoryTests
{
    public class UserRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<LibroDbContext> options;
        public UserRepositoryTests()
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
            using( var context = new LibroDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Dispose();
            }
        }

        [Fact]
        public async Task GetUserByIdAsync_WithNonExistingUserId_ReturnsNull()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _userRepository = new UserRepository(context);

                //Act
                var book = await _userRepository.GetUserByIdAsync(-1);

                //Assert
                Assert.Null(book);
            }
        }

        [Fact]
        public async Task GetUserByIdAsync_WithExistongUserId_ReturnsUser()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _userRepository = new UserRepository(context);

                //Act
                var user = await _userRepository.GetUserByIdAsync(1);

                //Assert
                Assert.IsType<User>(user);
            }
        }

        [Fact]
        public async Task UserExistsByIdAsync_WithNonExistingUserId_ReturnsFalseResult()
        {
            //Arrange

            using (var context = new LibroDbContext(options))
            {
                var _userRepository = new UserRepository(context);

                //Act
                var result = await _userRepository.UserExistsByIdAsync(-1);

                //Assert
                Assert.False(result);
            }
        }

        [Fact]
        public async Task UserExistsByIdAsync_WithExistingUserId_ReturnsTrueResult()
        {
            //Arrange

            using (var context = new LibroDbContext(options))
            {
                var _userRepository = new UserRepository(context);

                //Act
                var result = await _userRepository.UserExistsByIdAsync(1);

                //Assert
                Assert.True(result);
            }
        }

        [Fact]
        public async Task UserExistsByEmailAsync_WithExistingUserEmail_ReturnsTrueResult()
        {
            //Arrange

            using (var context = new LibroDbContext(options))
            {
                var _userRepository = new UserRepository(context);

                //Act
                var result = await _userRepository.UserExistsByEmailAsync("RaneenAsia101@gmail.com");

                //Assert
                Assert.True(result);
            }
        }

        [Fact]
        public async Task UserExistsByEmailAsync_WithNonExistingUserEmail_ReturnsFalseResult()
        {
            //Arrange

            using (var context = new LibroDbContext(options))
            {
                var _userRepository = new UserRepository(context);

                //Act
                var result = await _userRepository.UserExistsByEmailAsync("Test");

                //Assert
                Assert.False(result);
            }
        }

        [Fact]
        public async Task GetUserEmailsByIdsAsync_WithExistingUserIds_ReturnsListOfString()
        {
            //Arrange
            var userIds = new List<int>
            {
                1, 2, 3
            };

            using (var context = new LibroDbContext(options))
            {
                var _userRepository = new UserRepository(context);

                //Act
                var emails = await _userRepository.GetUserEmailsByIdsAsync(userIds);

                //Assert
                Assert.IsType<List<string>>(emails);
                Assert.Equal(3, emails.Count);
            }
        }

        [Fact]
        public async Task GetUserEmailsByIdsAsync_WithNonExistingUserIds_ReturnsEmptyListOfString()
        {
            //Arrange
            var userIds = new List<int>
            {
                -1
            };

            using (var context = new LibroDbContext(options))
            {
                var _userRepository = new UserRepository(context);

                //Act
                var emails = await _userRepository.GetUserEmailsByIdsAsync(userIds);

                //Assert
                Assert.Empty(emails);
            }
        }

        [Fact]
        public async Task CreateUserAsync_ValidUser_ReturnsCompletedResult()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _userRepository = new UserRepository(context);
                var user = new User { Username = "Test", Email = "Test@gmail.com", PasswordSalt = "Test", PasswordHash = "Test", Role = Role.Patron };

                //Act
                var result = await _userRepository.CreateUserAsync(user);

                //Assert
                Assert.IsType<Result>(result.Item2);
                Assert.Equal(Result.Completed, result.Item2);
                Assert.Equal(EntityState.Unchanged, context.Entry(user).State);
            }
        }


        [Fact]
        public async Task CreateUserAsync_NullUser_ReturnsFailedResult()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _userRepository = new UserRepository(context);

                //Act
                var result = await _userRepository.CreateUserAsync(null);

                //Assert
                Assert.IsType<Result>(result.Item2);
                Assert.Equal(Result.Failed, result.Item2);
            }
        }

        [Fact]
        public async Task ValidateUserCredentialsAsync_ValidCredentials_ReturnsCompletedResult()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _userRepository = new UserRepository(context);

                //Act
                var result = await _userRepository.ValidateUserCredentialsAsync("RaneenAsia101@gmail.com", "Raneen123");

                //Assert
                Assert.IsType<Result>(result.Item2);
                Assert.Equal(Result.Completed, result.Item2);
            }
        }

        [Theory]
        [InlineData("Test@gmail.com", "Test")]
        [InlineData("RaneenAsia101@gmail.com", "Test")]
        [InlineData("Test@gmail.com", "Raneen123")]
        public async Task ValidateUserCredentialsAsync_InValidCredentials_ReturnsFailedResult(string email, string password)
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _userRepository = new UserRepository(context);

                //Act
                var result = await _userRepository.ValidateUserCredentialsAsync(email, password);

                //Assert
                Assert.IsType<Result>(result.Item2);
                Assert.Equal(Result.Failed, result.Item2);
            }
        }
    }
}
