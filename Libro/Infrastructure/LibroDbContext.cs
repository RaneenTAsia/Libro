﻿using Domain.Entities;
using Infrastructure.Persistence.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
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
        }
    }
}