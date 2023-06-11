using Application.Configurations;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EntityConfigurations
{
    public class ModelCreator
    {
        public ModelBuilder ModelBuilder { get; set; }

        public ModelCreator(ModelBuilder modelBuilder)
        {
            this.ModelBuilder = modelBuilder;
        }

        public void ConfigureFluentValidations()
        {
            SetUpRelationships();
            ConfigureAuthorFluentValidations();
            ConfigureBookGenreFluentValidations();
            ConfigureBookFluentValidations();
            ConfigureBookToBookGenresFluentValidations();
            ConfigureAuthorToBookFluentValidations();
            ConfigureUserFluentValidations();
            ConfigureBookTransactionFluentValidations();
            ConfigureBookReservationFluentValidations();
        }

        public void SeedTables()
        {
            SeedAuthorsTable();
            SeedBookGenresTable();
            SeedBooksTable();
            SeedBookToBookGenresTable();
            SeedAuthorsToBooksTable();
            SeedUserTable();
            SeedBookTransactionsTable();
            SeedBookReservationsTable();
        }

        public void ConfigureAuthorFluentValidations()
        {
            ModelBuilder.Entity<Author>().Property(a => a.Name).IsRequired().HasMaxLength(30);
            ModelBuilder.Entity<Author>().Property(a => a.Description).HasMaxLength(255).HasDefaultValue(null);
        }

        public void ConfigureBookFluentValidations()
        {
            ModelBuilder.Entity<Book>().Property(b => b.Title).IsRequired().HasMaxLength(50);
            ModelBuilder.Entity<Book>().Property(b => b.BookStatus).IsRequired().HasDefaultValue(Status.Available);
            ModelBuilder.Entity<Book>().Property(b => b.PublishDate).HasColumnType("DATE");
        }

        public void ConfigureAuthorToBookFluentValidations()
        {
            ModelBuilder.Entity<AuthorToBook>().Property(ab => ab.AuthorId).IsRequired();
            ModelBuilder.Entity<AuthorToBook>().Property(ab => ab.BookId).IsRequired();
            ModelBuilder.Entity<AuthorToBook>().HasKey("AuthorId", "BookId");
        }

        public void ConfigureUserFluentValidations()
        {
            ModelBuilder.Entity<User>().Property(u => u.Role).IsRequired().HasDefaultValue(Role.Patron);
            ModelBuilder.Entity<User>().Property(u => u.Username).IsRequired().HasMaxLength(30);
            ModelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            ModelBuilder.Entity<User>().Property(u => u.Email).IsRequired().HasMaxLength(50);
            ModelBuilder.Entity<User>().Property(u => u.PasswordSalt).IsRequired();
            ModelBuilder.Entity<User>().Property(u => u.PasswordHash).IsRequired();
            ModelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        }

        public void ConfigureBookTransactionFluentValidations()
        {
            ModelBuilder.Entity<BookTransaction>().Property(bt => bt.UserId).IsRequired();
            ModelBuilder.Entity<BookTransaction>().Property(bt => bt.BorrowDate).IsRequired().HasColumnType("DATE");
            ModelBuilder.Entity<BookTransaction>().Property(bt => bt.DueDate).IsRequired().HasColumnType("DATE");
            ModelBuilder.Entity<BookTransaction>().Property(bt => bt.ReturnDate).HasColumnType("DATE");
            ModelBuilder.Entity<BookTransaction>().Property(bt => bt.Fine).HasDefaultValue(0M).HasColumnType("decimal(18,2)");
            ModelBuilder.Entity<BookTransaction>().Property(bt => bt.BookId).IsRequired();
        }

        public void ConfigureBookReservationFluentValidations()
        {
            ModelBuilder.Entity<BookReservation>().Property(br => br.UserId).IsRequired();
            ModelBuilder.Entity<BookReservation>().Property(br => br.ReserveDate).IsRequired().HasColumnType("DATE");
            ModelBuilder.Entity<BookReservation>().Property(br => br.BookId).IsRequired();
        }

        public void ConfigureBookGenreFluentValidations()
        {
            ModelBuilder.Entity<BookGenre>().Property(bg => bg.Genre).IsRequired().HasMaxLength(30);
        }
        public void ConfigureBookToBookGenresFluentValidations()
        {
            ModelBuilder.Entity<BookToBookGenre>().Property(bg => bg.BookGenreId).IsRequired();
            ModelBuilder.Entity<BookToBookGenre>().Property(bg => bg.BookId).IsRequired();
            ModelBuilder.Entity<BookToBookGenre>().HasKey("BookId", "BookGenreId");
        }

        public void SetUpRelationships()
        {
            ModelBuilder.Entity<Author>()
                .HasMany(a => a.WrittenBooks)
                .WithMany(a => a.Authors)
                .UsingEntity<AuthorToBook>
                (
                    ab => ab.HasOne(a => a.Book)
                    .WithMany()
                    .HasForeignKey(a => a.BookId),
                    ab => ab.HasOne(a => a.Author)
                         .WithMany()
                         .HasForeignKey(a => a.AuthorId),
                    ab =>
                    {
                        ab.ToTable("AuthorsToBooks");
                        ab.HasKey(a => new { a.AuthorId, a.BookId });
                    }
                );

            ModelBuilder.Entity<Book>()
                .HasMany(a => a.BookGenres)
                .WithMany(a => a.Books)
                .UsingEntity<BookToBookGenre>
                (
                    ab => ab.HasOne(a => a.BookGenre)
                    .WithMany()
                    .HasForeignKey(a => a.BookGenreId),
                    ab => ab.HasOne(a => a.Book)
                         .WithMany()
                         .HasForeignKey(a => a.BookId),
                    ab =>
                    {
                        ab.ToTable("BooksToBookGenres");
                        ab.HasKey(a => new { a.BookId, a.BookGenreId });
                    }
                );
        }

        public void SeedAuthorsTable()
        {
            List<Author> Authors = new List<Author>()
            {new Author{AuthorId = 1, Name = "J.K.Rowling", Description = "One of the first internationally known female writers who had to use a disguised name to have their books published."},
            new Author{AuthorId = 2, Name = "Edgar Allen Poe", Description = "Born in London, this author was very well know for his mysterious short stories and mysterious death. "},
            new Author{AuthorId = 3, Name = "Stephen King", Description= "Known for his creepy and horror novels that have unecpected plot twists"}
            };

            ModelBuilder.Entity<Author>().HasData(Authors);
        }

        public void SeedBookGenresTable()
        {
            List<BookGenre> bookGenres = new List<BookGenre>()
            {
                new BookGenre{ BookGenreId = Genre.Romance, Genre = "Romance"},
                new BookGenre{ BookGenreId = Genre.Mystery, Genre = "Mystery"},
                new BookGenre{ BookGenreId = Genre.Horror, Genre = "Horror"},
                new BookGenre{ BookGenreId = Genre.Comedy, Genre = "Comedy"},
                new BookGenre{ BookGenreId = Genre.Children, Genre = "Children"},
                new BookGenre{ BookGenreId = Genre.Action, Genre = "Action"},
                new BookGenre{ BookGenreId = Genre.Fiction, Genre = "Fiction"},
                new BookGenre{ BookGenreId = Genre.Historical, Genre = "Historical"},
                new BookGenre{ BookGenreId = Genre.NonFiction, Genre = "NonFiction"},
                new BookGenre{ BookGenreId = Genre.Science, Genre = "Science"},
                new BookGenre{ BookGenreId = Genre.ScienceFiction, Genre = "Science Fition"},
                new BookGenre{ BookGenreId = Genre.Fantasy, Genre = "Fantasy"},
                new BookGenre{ BookGenreId = Genre.RealisticFiction, Genre = "Realistic Fiction"}
            };

            ModelBuilder.Entity<BookGenre>().HasData(bookGenres);
        }

        public void SeedBooksTable()
        {
            List<Book> Books = new List<Book>()
            {
                new Book{ BookId = 1, Title = "The Casque of Amontillado", BookStatus = (int)Status.Borrowed},
                new Book{ BookId = 2, Title = "The Masque of Red Death", BookStatus = (int) Status.Borrowed},
                new Book{ BookId = 3, Title = "IT", BookStatus = (int) Status.Reserved},
                new Book{ BookId = 4, Title = "Harry Potter and The Chamber of Secrets"}
            };

            ModelBuilder.Entity<Book>().HasData(Books);
        }

        public void SeedBookToBookGenresTable()
        {
            List<BookToBookGenre> BooksToBooksGenres = new List<BookToBookGenre>()
            {
                new BookToBookGenre{ BookId = 1, BookGenreId = Genre.Mystery},
                new BookToBookGenre{ BookId = 1, BookGenreId = Genre.Fiction},
                new BookToBookGenre{ BookId = 2, BookGenreId = Genre.Mystery},
                new BookToBookGenre{ BookId = 2, BookGenreId = Genre.Fiction},
                new BookToBookGenre{ BookId = 3, BookGenreId = Genre.Mystery},
                new BookToBookGenre{ BookId = 3, BookGenreId = Genre.Horror},
                new BookToBookGenre{ BookId = 4, BookGenreId = Genre.Fantasy},
                new BookToBookGenre{ BookId = 4, BookGenreId = Genre.Action}
            };

            ModelBuilder.Entity<BookToBookGenre>().HasData(BooksToBooksGenres);
        }

        public void SeedAuthorsToBooksTable()
        {
            List<AuthorToBook> authorsToBooks = new List<AuthorToBook>()
            {
                new AuthorToBook{ AuthorId = 1, BookId = 4},
                new AuthorToBook{ AuthorId = 2, BookId = 1},
                new AuthorToBook{ AuthorId = 2, BookId = 2},
                new AuthorToBook{ AuthorId = 3, BookId = 3 }
            };

            ModelBuilder.Entity<AuthorToBook>().HasData(authorsToBooks);
        }

        public void SeedUserTable()
        {
            var salt = PasswordHasher.GenerateSalt();
            List<User> users = new List<User>()
            {
                new User{ UserId = 1, Username = "Rami", PasswordSalt = salt, PasswordHash = PasswordHasher.ComputeHash("Rami123", salt, 3), Email = "Rami@gmail.com", Role = Role.Administrator},
                new User{ UserId = 2, Username = "Rayyan", PasswordSalt = salt, PasswordHash = PasswordHasher.ComputeHash("Rayyan123", salt, 3), Email = "RayyanTawfieg@gmail.com", Role = Role.Librarian},
                new User{ UserId = 3, Username = "Raneen", PasswordSalt = salt, PasswordHash = PasswordHasher.ComputeHash("Raneen123", salt, 3), Email = "Raneenasia101@gmail.com", Role = Role.Patron},
                 new User{ UserId = 4, Username = "Reema", PasswordSalt = salt, PasswordHash = PasswordHasher.ComputeHash("Reema123", salt, 3), Email = "Reema@gmail.com", Role = Role.Patron}
            };

            ModelBuilder.Entity<User>().HasData(users);
        }

        public void SeedBookTransactionsTable()
        {
            List<BookTransaction> bookTransactions = new List<BookTransaction>()
            {
                new BookTransaction{ BookTransactionId = 1, BookId = 1, UserId = 3, BorrowDate= new DateTime(2023, 05, 25), DueDate = new DateTime( 2023, 06, 25)},
                new BookTransaction{ BookTransactionId = 2, BookId = 2, UserId = 3, BorrowDate = new DateTime(2023, 04, 30), DueDate = new DateTime( 2023, 05, 30)}
            };

            ModelBuilder.Entity<BookTransaction>().HasData(bookTransactions);
        }
        public void SeedBookReservationsTable()
        {
            List<BookReservation> bookReservations = new List<BookReservation>()
            {
                new BookReservation{ BookReservationId = 1, BookId = 1, UserId = 4, ReserveDate = DateTime.Now.Date}
            };

            ModelBuilder.Entity<BookReservation>().HasData(bookReservations);
        }

        public void MapViews()
        {
            ModelBuilder.Entity<ViewBooks>().HasNoKey().ToView("ViewBooks");
            ModelBuilder.Entity<ViewOverdueBookDetails>().HasNoKey().ToView("ViewOverdueBooksDetails");
            ModelBuilder.Entity<UserBorrowingHistoryFunctionResult>().HasNoKey();
        }
    }
}
