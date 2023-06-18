﻿// <auto-generated />
using System;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(LibroDbContext))]
    partial class LibroDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Author", b =>
                {
                    b.Property<int>("AuthorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AuthorId"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("AuthorId");

                    b.ToTable("Authors");

                    b.HasData(
                        new
                        {
                            AuthorId = 1,
                            Description = "One of the first internationally known female writers who had to use a disguised name to have their books published.",
                            Name = "J.K.Rowling"
                        },
                        new
                        {
                            AuthorId = 2,
                            Description = "Born in London, this author was very well know for his mysterious short stories and mysterious death. ",
                            Name = "Edgar Allen Poe"
                        },
                        new
                        {
                            AuthorId = 3,
                            Description = "Known for his creepy and horror novels that have unecpected plot twists",
                            Name = "Stephen King"
                        });
                });

            modelBuilder.Entity("Domain.Entities.AuthorToBook", b =>
                {
                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.HasKey("AuthorId", "BookId");

                    b.HasIndex("BookId");

                    b.ToTable("AuthorsToBooks", (string)null);

                    b.HasData(
                        new
                        {
                            AuthorId = 1,
                            BookId = 4
                        },
                        new
                        {
                            AuthorId = 2,
                            BookId = 1
                        },
                        new
                        {
                            AuthorId = 2,
                            BookId = 2
                        },
                        new
                        {
                            AuthorId = 3,
                            BookId = 3
                        });
                });

            modelBuilder.Entity("Domain.Entities.Book", b =>
                {
                    b.Property<int>("BookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookId"));

                    b.Property<int>("BookStatus")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PageAmount")
                        .HasColumnType("int");

                    b.Property<DateTime>("PublishDate")
                        .HasColumnType("DATE");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("BookId");

                    b.ToTable("Books");

                    b.HasData(
                        new
                        {
                            BookId = 1,
                            BookStatus = 3,
                            PageAmount = 0,
                            PublishDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "The Casque of Amontillado"
                        },
                        new
                        {
                            BookId = 2,
                            BookStatus = 3,
                            PageAmount = 0,
                            PublishDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "The Masque of Red Death"
                        },
                        new
                        {
                            BookId = 3,
                            BookStatus = 2,
                            PageAmount = 0,
                            PublishDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "IT"
                        },
                        new
                        {
                            BookId = 4,
                            BookStatus = 0,
                            PageAmount = 0,
                            PublishDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Title = "Harry Potter and The Chamber of Secrets"
                        });
                });

            modelBuilder.Entity("Domain.Entities.BookGenre", b =>
                {
                    b.Property<int>("BookGenreId")
                        .HasColumnType("int");

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("BookGenreId");

                    b.ToTable("BookGenres");

                    b.HasData(
                        new
                        {
                            BookGenreId = 1,
                            Genre = "Romance"
                        },
                        new
                        {
                            BookGenreId = 10,
                            Genre = "Mystery"
                        },
                        new
                        {
                            BookGenreId = 12,
                            Genre = "Horror"
                        },
                        new
                        {
                            BookGenreId = 2,
                            Genre = "Comedy"
                        },
                        new
                        {
                            BookGenreId = 3,
                            Genre = "Children"
                        },
                        new
                        {
                            BookGenreId = 7,
                            Genre = "Action"
                        },
                        new
                        {
                            BookGenreId = 5,
                            Genre = "Fiction"
                        },
                        new
                        {
                            BookGenreId = 11,
                            Genre = "Historical"
                        },
                        new
                        {
                            BookGenreId = 6,
                            Genre = "NonFiction"
                        },
                        new
                        {
                            BookGenreId = 4,
                            Genre = "Science"
                        },
                        new
                        {
                            BookGenreId = 8,
                            Genre = "Science Fition"
                        },
                        new
                        {
                            BookGenreId = 13,
                            Genre = "Fantasy"
                        },
                        new
                        {
                            BookGenreId = 9,
                            Genre = "Realistic Fiction"
                        });
                });

            modelBuilder.Entity("Domain.Entities.BookReservation", b =>
                {
                    b.Property<int>("BookReservationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookReservationId"));

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReserveDate")
                        .HasColumnType("DATE");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("BookReservationId");

                    b.HasIndex("BookId");

                    b.HasIndex("UserId");

                    b.ToTable("BookReservations");

                    b.HasData(
                        new
                        {
                            BookReservationId = 1,
                            BookId = 1,
                            ReserveDate = new DateTime(2023, 6, 18, 0, 0, 0, 0, DateTimeKind.Local),
                            UserId = 4
                        });
                });

            modelBuilder.Entity("Domain.Entities.BookReviewsFunctionResult", b =>
                {
                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<string>("ReviewContent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("BookReviewsFunctionResult");
                });

            modelBuilder.Entity("Domain.Entities.BookToBookGenre", b =>
                {
                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<int>("BookGenreId")
                        .HasColumnType("int");

                    b.HasKey("BookId", "BookGenreId");

                    b.HasIndex("BookGenreId");

                    b.ToTable("BooksToBookGenres", (string)null);

                    b.HasData(
                        new
                        {
                            BookId = 1,
                            BookGenreId = 10
                        },
                        new
                        {
                            BookId = 1,
                            BookGenreId = 5
                        },
                        new
                        {
                            BookId = 2,
                            BookGenreId = 10
                        },
                        new
                        {
                            BookId = 2,
                            BookGenreId = 5
                        },
                        new
                        {
                            BookId = 3,
                            BookGenreId = 10
                        },
                        new
                        {
                            BookId = 3,
                            BookGenreId = 12
                        },
                        new
                        {
                            BookId = 4,
                            BookGenreId = 13
                        },
                        new
                        {
                            BookId = 4,
                            BookGenreId = 7
                        });
                });

            modelBuilder.Entity("Domain.Entities.BookTransaction", b =>
                {
                    b.Property<int>("BookTransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookTransactionId"));

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<DateTime>("BorrowDate")
                        .HasColumnType("DATE");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("DATE");

                    b.Property<decimal>("Fine")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(18,2)")
                        .HasDefaultValue(0m);

                    b.Property<DateTime?>("ReturnDate")
                        .HasColumnType("DATE");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("BookTransactionId");

                    b.HasIndex("BookId");

                    b.HasIndex("UserId");

                    b.ToTable("BookTransactions");

                    b.HasData(
                        new
                        {
                            BookTransactionId = 1,
                            BookId = 1,
                            BorrowDate = new DateTime(2023, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DueDate = new DateTime(2023, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Fine = 0m,
                            UserId = 3
                        },
                        new
                        {
                            BookTransactionId = 2,
                            BookId = 2,
                            BorrowDate = new DateTime(2023, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DueDate = new DateTime(2023, 5, 30, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Fine = 0m,
                            UserId = 3
                        });
                });

            modelBuilder.Entity("Domain.Entities.ReadingItem", b =>
                {
                    b.Property<int>("ReadingItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReadingItemId"));

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<int>("ReadingListId")
                        .HasColumnType("int");

                    b.HasKey("ReadingItemId");

                    b.HasIndex("BookId");

                    b.HasIndex("ReadingListId");

                    b.ToTable("ReadingItems");

                    b.HasData(
                        new
                        {
                            ReadingItemId = 1,
                            BookId = 1,
                            ReadingListId = 1
                        });
                });

            modelBuilder.Entity("Domain.Entities.ReadingList", b =>
                {
                    b.Property<int>("ReadingListId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReadingListId"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(max)")
                        .HasDefaultValue("Reading List");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ReadingListId");

                    b.HasIndex("UserId");

                    b.ToTable("ReadingLists");

                    b.HasData(
                        new
                        {
                            ReadingListId = 1,
                            Title = "Calmimg",
                            UserId = 4
                        });
                });

            modelBuilder.Entity("Domain.Entities.ReadingListItemFunctionResult", b =>
                {
                    b.Property<string>("Authors")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<int>("BookStatus")
                        .HasColumnType("int");

                    b.Property<string>("Genres")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("ReadingListItemFunctionResult");
                });

            modelBuilder.Entity("Domain.Entities.Review", b =>
                {
                    b.Property<int>("ReviewId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReviewId"));

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<string>("ReviewContent")
                        .HasMaxLength(2147483647)
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ReviewId");

                    b.HasIndex("BookId");

                    b.HasIndex("UserId", "BookId")
                        .IsUnique();

                    b.ToTable("Reviews");

                    b.HasData(
                        new
                        {
                            ReviewId = 1,
                            BookId = 3,
                            Rating = 5,
                            ReviewContent = "Inspiring boook",
                            UserId = 4
                        });
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(3);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            Email = "Rami@gmail.com",
                            PasswordHash = "hVi2sPGUMgH7Log3LxDhiKC2VeMvX1E5+dWCHlCrLlQ=",
                            PasswordSalt = "vmhYinW5OIRhBJy5Xmdp9w==",
                            Role = 1,
                            Username = "Rami"
                        },
                        new
                        {
                            UserId = 2,
                            Email = "RayyanTawfieg@gmail.com",
                            PasswordHash = "4JzKGQl54kd+0FK/4Ub3289orQJP1Y0sMhNeyAWOMvE=",
                            PasswordSalt = "vmhYinW5OIRhBJy5Xmdp9w==",
                            Role = 2,
                            Username = "Rayyan"
                        },
                        new
                        {
                            UserId = 3,
                            Email = "Raneenasia101@gmail.com",
                            PasswordHash = "U65xncqgYLqAAetQ/De/W4AINRy8dzC4vhh2Cr2bK30=",
                            PasswordSalt = "vmhYinW5OIRhBJy5Xmdp9w==",
                            Role = 3,
                            Username = "Raneen"
                        },
                        new
                        {
                            UserId = 4,
                            Email = "Reema@gmail.com",
                            PasswordHash = "p3nf0HhjYTsRv4KhkV2kp0OK5C72SNj/Frny/844M7I=",
                            PasswordSalt = "vmhYinW5OIRhBJy5Xmdp9w==",
                            Role = 3,
                            Username = "Reema"
                        });
                });

            modelBuilder.Entity("Domain.Entities.UserBorrowingHistoryFunctionResult", b =>
                {
                    b.Property<string>("Authors")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<DateTime>("BorrowDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("Fine")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("ReturnDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("UserBorrowingHistoryFunctionResult");
                });

            modelBuilder.Entity("Domain.Entities.ViewBooks", b =>
                {
                    b.Property<string>("Authors")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<int>("BookStatus")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Genres")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PageAmount")
                        .HasColumnType("int");

                    b.Property<DateTime>("PublishDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable((string)null);

                    b.ToView("ViewBooks", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.ViewOverdueBookDetails", b =>
                {
                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.ToTable((string)null);

                    b.ToView("ViewOverdueBooksDetails", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.AuthorToBook", b =>
                {
                    b.HasOne("Domain.Entities.Author", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Book");
                });

            modelBuilder.Entity("Domain.Entities.BookReservation", b =>
                {
                    b.HasOne("Domain.Entities.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.BookToBookGenre", b =>
                {
                    b.HasOne("Domain.Entities.BookGenre", "BookGenre")
                        .WithMany()
                        .HasForeignKey("BookGenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("BookGenre");
                });

            modelBuilder.Entity("Domain.Entities.BookTransaction", b =>
                {
                    b.HasOne("Domain.Entities.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.ReadingItem", b =>
                {
                    b.HasOne("Domain.Entities.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.ReadingList", "ReadingList")
                        .WithMany("ReadingItems")
                        .HasForeignKey("ReadingListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("ReadingList");
                });

            modelBuilder.Entity("Domain.Entities.ReadingList", b =>
                {
                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.Review", b =>
                {
                    b.HasOne("Domain.Entities.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.ReadingList", b =>
                {
                    b.Navigation("ReadingItems");
                });
#pragma warning restore 612, 618
        }
    }
}
