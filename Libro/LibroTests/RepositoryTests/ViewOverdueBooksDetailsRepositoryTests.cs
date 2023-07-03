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
    public class ViewOverdueBooksDetailsRepositoryTests : IDisposable
    {
        private readonly DbContextOptions<LibroDbContext> options;
        public ViewOverdueBooksDetailsRepositoryTests()
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
        public async Task GetOverdueBooksAsync_ReturnsListOfViewOverdueBooks()
        {
            //Arrange
            using (var context = new LibroDbContext(options))
            {
                var _viewOverdueBooksDetailsRepository = new ViewOverdueBooksDetailsRepository(context);

                //Act
                var books = await _viewOverdueBooksDetailsRepository.GetOverdueBooksAsync();

                //Assert
                Assert.IsType<List<ViewOverdueBookDetails>>(books);
            }
        }
    }
}
