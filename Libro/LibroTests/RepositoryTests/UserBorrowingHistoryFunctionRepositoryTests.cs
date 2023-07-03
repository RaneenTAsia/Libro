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
    public class UserBorrowingHistoryFunctionRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<LibroDbContext> options;
        public UserBorrowingHistoryFunctionRepositoryTests()
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
        public void GetUserBorrowingHistory_GetBorrowingHistoryOfExistingUserId_ReturnsListOfUserBorrowingHistoryFunctionResult()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _userBorrowingHistoryFunctionRepository  = new UserBorrowingHistoryFunctionRepository(context);

                //Act
                var history = _userBorrowingHistoryFunctionRepository.GetUserBorrowingHistory(1);

                //Assert
                Assert.IsType<List<UserBorrowingHistoryFunctionResult>>(history);
            }
        }

        [Fact]
        public void GetUserBorrowingHistory_GetBorrowingHistoryOfNonExistingUserId_ReturnsEmptyListResult()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _userBorrowingHistoryFunctionRepository = new UserBorrowingHistoryFunctionRepository(context);

                //Act
                var history = _userBorrowingHistoryFunctionRepository.GetUserBorrowingHistory(-1);

                //Assert
                Assert.Empty(history);
            }
        }
    }
}
