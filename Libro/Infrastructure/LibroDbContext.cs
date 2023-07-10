using Domain.Entities;
using Infrastructure.Persistence.EntityConfigurations;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class LibroDbContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookGenre> BookGenres { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookToBookGenre> BooksToBookGenres { get; set; }
        public DbSet<AuthorToBook> AuthorsToBooks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BookTransaction> BookTransactions { get; set; }
        public DbSet<BookReservation> BookReservations { get; set; }
        public DbSet<ReadingList> ReadingLists { get; set; }
        public DbSet<ReadingItem> ReadingItems { get; set; }
        public string BookAuthorsFunctionResult(int bookId) => throw new NotSupportedException();
        public string BookGenresFunctionResult (int bookId) => throw new NotSupportedException();
        public DbSet<ViewBooks> ViewBooks { get; set; }
        public DbSet<ViewOverdueBookDetails> ViewOverdueBooksDetails { get; set; }
        public IQueryable<UserBorrowingHistoryFunctionResult> UserBorrowingHistoryFunctionResults(int UserId) => FromExpression(expression: () => UserBorrowingHistoryFunctionResults(UserId));
        public IQueryable<ReadingListItemFunctionResult> ReadingListItemsFunctionResults(int ReadingListId) => FromExpression(expression: () => ReadingListItemsFunctionResults(ReadingListId));
        public DbSet<Review> Reviews { get; set; }
        public IQueryable<BookReviewsFunctionResult> BookReviewsFunctionResults(int BookId) => FromExpression(expression: () => BookReviewsFunctionResults(BookId));
        public DbSet<BookTransactionJob> BookTransactionJobs { get; set; }
        public DbSet<BookReservationJob> BookReservationJobs { get; set; }

        public LibroDbContext()
        {
        }

        public LibroDbContext(DbContextOptions<LibroDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = Libro")
                    .LogTo(Console.WriteLine, new[]
                    {DbLoggerCategory.Database.Command.Name },
                     LogLevel.Information).EnableSensitiveDataLogging();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ModelCreator creator = new ModelCreator(modelBuilder);
            creator.ConfigureFluentValidations();
            creator.SeedTables();

            modelBuilder.HasDbFunction(typeof(LibroDbContext).GetMethod(nameof(BookGenresFunctionResult), new[] { typeof(int) }))
                .HasName("fnBookGenres");

            modelBuilder.HasDbFunction(typeof(LibroDbContext).GetMethod(nameof(BookAuthorsFunctionResult), new[] { typeof(int) }))
                 .HasName("fnBookAuthors");

            modelBuilder.HasDbFunction(typeof(LibroDbContext).GetMethod(nameof(UserBorrowingHistoryFunctionResults), new[] { typeof(int) }))
                .HasName("fnUserBorrowingHistory");

            modelBuilder.HasDbFunction(typeof(LibroDbContext).GetMethod(nameof(ReadingListItemsFunctionResults), new[] { typeof(int) }))
                 .HasName("fnReadingListItems");

            modelBuilder.HasDbFunction(typeof(LibroDbContext).GetMethod(nameof(BookReviewsFunctionResults), new[] { typeof(int) }))
                 .HasName("fnBookReviews");

            creator.MapViewsAndFunctions();
        }
    }
}
