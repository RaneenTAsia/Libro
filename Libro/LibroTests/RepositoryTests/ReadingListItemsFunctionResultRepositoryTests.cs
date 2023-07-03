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
    public class ReadingListItemsFunctionResultRepositoryTests
    {
        private readonly DbContextOptions<LibroDbContext> options;
        public ReadingListItemsFunctionResultRepositoryTests()
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
        public async Task GetReadingList_WithExistingReadingListId_ReturnsListOReadingListItemFunctionResult()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _readingListItemsFunctionRepository = new ReadingListItemsFunctionRepository(context);

                //Act
                var results = await _readingListItemsFunctionRepository.GetReadingListAsync(1);

                //Assert
                Assert.IsType<List<ReadingListItemFunctionResult>>(results);
            }
        }

        [Fact]
        public async Task GetReadingList_WithNonExistingReadingListId_ReturnsListOReadingListItemFunctionResult()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _readingListItemsFunctionRepository = new ReadingListItemsFunctionRepository(context);

                //Act
                var results = await _readingListItemsFunctionRepository.GetReadingListAsync(-1);

                //Assert
                Assert.IsType<List<ReadingListItemFunctionResult>>(results);
                Assert.Empty(results);
            }
        }
    }
}
