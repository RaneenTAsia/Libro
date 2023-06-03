using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReservationsTableAndUpdateData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 3,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 2);

            migrationBuilder.AlterColumn<int>(
                name: "BookStatus",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Users_Email",
                table: "Users",
                column: "Email");

            migrationBuilder.CreateTable(
                name: "BookReservations",
                columns: table => new
                {
                    BookReservationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ReserveDate = table.Column<DateTime>(type: "DATE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookReservations", x => x.BookReservationId);
                    table.ForeignKey(
                        name: "FK_BookReservations_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookReservations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 1,
                column: "Genre",
                value: "Romance");

            migrationBuilder.UpdateData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 2,
                column: "Genre",
                value: "Comedy");

            migrationBuilder.UpdateData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 3,
                column: "Genre",
                value: "Children");

            migrationBuilder.UpdateData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 4,
                column: "Genre",
                value: "Science");

            migrationBuilder.UpdateData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 5,
                column: "Genre",
                value: "Fiction");

            migrationBuilder.UpdateData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 6,
                column: "Genre",
                value: "NonFiction");

            migrationBuilder.UpdateData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 7,
                column: "Genre",
                value: "Action");

            migrationBuilder.UpdateData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 8,
                column: "Genre",
                value: "Science Fition");

            migrationBuilder.UpdateData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 9,
                column: "Genre",
                value: "Realistic Fiction");

            migrationBuilder.UpdateData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 10,
                column: "Genre",
                value: "Mystery");

            migrationBuilder.UpdateData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 11,
                column: "Genre",
                value: "Historical");

            migrationBuilder.UpdateData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 12,
                column: "Genre",
                value: "Horror");

            migrationBuilder.InsertData(
                table: "BookGenres",
                columns: new[] { "BookGenreId", "Genre" },
                values: new object[] { 13, "Fantasy" });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 1,
                column: "BookStatus",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 2,
                column: "BookStatus",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 3,
                column: "BookStatus",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 4,
                column: "BookStatus",
                value: 1);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Email", "PasswordHash", "PasswordSalt", "Role", "Username" },
                values: new object[] { 4, "Reema@gmail.com", "YqWnNabO8fhTsO2ktWJd9PFSPeybSxYrZ3zM5rqbzA4=", "gY+S8TX9ZA2oG1coRz6R/A==", 3, "Reema" });

            migrationBuilder.InsertData(
                table: "BookReservations",
                columns: new[] { "BookReservationId", "BookId", "ReserveDate", "UserId" },
                values: new object[] { 1, 1, new DateTime(2023, 6, 1, 0, 0, 0, 0, DateTimeKind.Local), 4 });

            migrationBuilder.InsertData(
                table: "BooksToBookGenres",
                columns: new[] { "BookGenreId", "BookId" },
                values: new object[,]
                {
                    { 5, 1 },
                    { 10, 1 },
                    { 5, 2 },
                    { 10, 2 },
                    { 10, 3 },
                    { 12, 3 },
                    { 7, 4 },
                    { 13, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookReservations_BookId",
                table: "BookReservations",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookReservations_UserId",
                table: "BookReservations",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookReservations");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Users_Email",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Username",
                table: "Users");

            migrationBuilder.DeleteData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 4);

            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 2,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 3);

            migrationBuilder.AlterColumn<int>(
                name: "BookStatus",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.UpdateData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 1,
                column: "Genre",
                value: "Comedy");

            migrationBuilder.UpdateData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 2,
                column: "Genre",
                value: "Children");

            migrationBuilder.UpdateData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 3,
                column: "Genre",
                value: "Science");

            migrationBuilder.UpdateData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 4,
                column: "Genre",
                value: "Fiction");

            migrationBuilder.UpdateData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 5,
                column: "Genre",
                value: "NonFiction");

            migrationBuilder.UpdateData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 6,
                column: "Genre",
                value: "Action");

            migrationBuilder.UpdateData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 7,
                column: "Genre",
                value: "Science Fition");

            migrationBuilder.UpdateData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 8,
                column: "Genre",
                value: "Realistic Fiction");

            migrationBuilder.UpdateData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 9,
                column: "Genre",
                value: "Mystery");

            migrationBuilder.UpdateData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 10,
                column: "Genre",
                value: "Historical");

            migrationBuilder.UpdateData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 11,
                column: "Genre",
                value: "Horror");

            migrationBuilder.UpdateData(
                table: "BookGenres",
                keyColumn: "BookGenreId",
                keyValue: 12,
                column: "Genre",
                value: "Fantasy");

            migrationBuilder.InsertData(
                table: "BookGenres",
                columns: new[] { "BookGenreId", "Genre" },
                values: new object[] { 0, "Romance" });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 1,
                column: "BookStatus",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 2,
                column: "BookStatus",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 3,
                column: "BookStatus",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 4,
                column: "BookStatus",
                value: 0);

            migrationBuilder.DeleteData(
                table: "BooksToBookGenres",
                keyColumns: new[] { "BookGenreId", "BookId" },
                keyValues: new object[] { 5, 1 });

            migrationBuilder.DeleteData(
                table: "BooksToBookGenres",
                keyColumns: new[] { "BookGenreId", "BookId" },
                keyValues: new object[] { 10, 1 });

            migrationBuilder.DeleteData(
                table: "BooksToBookGenres",
                keyColumns: new[] { "BookGenreId", "BookId" },
                keyValues: new object[] { 5, 2 });

            migrationBuilder.DeleteData(
                table: "BooksToBookGenres",
                keyColumns: new[] { "BookGenreId", "BookId" },
                keyValues: new object[] { 10, 2 });

            migrationBuilder.DeleteData(
                table: "BooksToBookGenres",
                keyColumns: new[] { "BookGenreId", "BookId" },
                keyValues: new object[] { 10, 3 });

            migrationBuilder.DeleteData(
                table: "BooksToBookGenres",
                keyColumns: new[] { "BookGenreId", "BookId" },
                keyValues: new object[] { 12, 3 });

            migrationBuilder.DeleteData(
                table: "BooksToBookGenres",
                keyColumns: new[] { "BookGenreId", "BookId" },
                keyValues: new object[] { 7, 4 });

            migrationBuilder.DeleteData(
                table: "BooksToBookGenres",
                keyColumns: new[] { "BookGenreId", "BookId" },
                keyValues: new object[] { 13, 4 });
        }
    }
}
