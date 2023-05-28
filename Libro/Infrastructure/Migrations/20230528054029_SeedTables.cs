using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "AuthorId", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "One of the first internationally known female writers who had to use a disguised name to have their books published.", "J.K.Rowling" },
                    { 2, "Born in London, this author was very well know for his mysterious short stories and mysterious death. ", "Edgar Allen Poe" },
                    { 3, "Known for his creepy and horror novels that have unecpected plot twists", "Stephen King" }
                });

            migrationBuilder.InsertData(
                table: "BookGenres",
                columns: new[] { "BookGenreId", "Genre" },
                values: new object[,]
                {
                    { 0, "Romance" },
                    { 1, "Comedy" },
                    { 2, "Children" },
                    { 3, "Science" },
                    { 4, "Fiction" },
                    { 5, "NonFiction" },
                    { 6, "Action" },
                    { 7, "Science Fition" },
                    { 8, "Realistic Fiction" },
                    { 9, "Mystery" },
                    { 10, "Historical" },
                    { 11, "Horror" },
                    { 12, "Fantasy" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookId", "Description", "PageAmount", "PublishDate", "Title" },
                values: new object[,]
                {
                    { 1, null, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Casque of Amontillado" },
                    { 2, null, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Masque of Red Death" },
                    { 3, null, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "IT" },
                    { 4, null, 0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Harry Potter and The Chamber of Secrets" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "PasswordHash", "PasswordSalt", "Username" },
                values: new object[] { 1, "Rami@gmail.com", "VXrG9IvunWZg5YRoF1RyXr8f4EEoUv7/qOsSAXpeUhw=", "3xxxlRX/mVEA1fLXpeXXBw==", "Rami" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "PasswordHash", "PasswordSalt", "Role", "Username" },
                values: new object[,]
                {
                    { 2, "RayyanTawfieg@gmail.com", "X88lHyDC9YtO1Rthn6ZAxdDi2AIV71S8ULXM4ayd9pA=", "3xxxlRX/mVEA1fLXpeXXBw==", 1, "Rayyan" },
                    { 3, "Raneenasia101@gmail.com", "TUZytoJf+gvgM1nFZtoO3/G+SC7sSDCOgOtyblDB5Ws=", "3xxxlRX/mVEA1fLXpeXXBw==", 2, "Raneen" }
                });

            migrationBuilder.InsertData(
                table: "AuthorsToBooks",
                columns: new[] { "AuthorId", "BookId" },
                values: new object[,]
                {
                    { 1, 4 },
                    { 2, 1 },
                    { 2, 2 },
                    { 3, 3 }
                });

            migrationBuilder.InsertData(
                table: "BookTransactions",
                columns: new[] { "BookTransactionId", "BookId", "BorrowDate", "DueDate", "ReturnDate", "UserId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2023, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3 },
                    { 2, 2, new DateTime(2023, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 5, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AuthorsToBooks",
                keyColumns: new[] { "AuthorId", "BookId" },
                keyValues: new object[] { 1, 4 });

            migrationBuilder.DeleteData(
                table: "AuthorsToBooks",
                keyColumns: new[] { "AuthorId", "BookId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "AuthorsToBooks",
                keyColumns: new[] { "AuthorId", "BookId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "AuthorsToBooks",
                keyColumns: new[] { "AuthorId", "BookId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 0);

            migrationBuilder.DeleteData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "BookTransactions",
                keyColumn: "BookTransactionId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BookTransactions",
                keyColumn: "BookTransactionId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "AuthorId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "AuthorId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "AuthorId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3);
        }
    }
}
