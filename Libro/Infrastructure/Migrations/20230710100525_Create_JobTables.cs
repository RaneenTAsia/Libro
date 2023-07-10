using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Create_JobTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BookReservationEmailJobs",
                columns: table => new
                {
                    BookReservationEmailJobId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookReservationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookReservationEmailJobs", x => x.BookReservationEmailJobId);
                    table.ForeignKey(
                        name: "FK_BookReservationEmailJobs_BookReservations_BookReservationId",
                        column: x => x.BookReservationId,
                        principalTable: "BookReservations",
                        principalColumn: "BookReservationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookReservationJobs",
                columns: table => new
                {
                    BookReservationJobId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookReservationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookReservationJobs", x => x.BookReservationJobId);
                    table.ForeignKey(
                        name: "FK_BookReservationJobs_BookReservations_BookReservationId",
                        column: x => x.BookReservationId,
                        principalTable: "BookReservations",
                        principalColumn: "BookReservationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookTransactionEmailJobs",
                columns: table => new
                {
                    BookTransactionEmailJobId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookTransactionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookTransactionEmailJobs", x => x.BookTransactionEmailJobId);
                    table.ForeignKey(
                        name: "FK_BookTransactionEmailJobs_BookTransactions_BookTransactionId",
                        column: x => x.BookTransactionId,
                        principalTable: "BookTransactions",
                        principalColumn: "BookTransactionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "BookReservations",
                keyColumn: "BookReservationId",
                keyValue: 1,
                column: "ReserveDate",
                value: new DateTime(2023, 7, 10, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "o06RFDkRzgtFYgN/1/iSC3pyYki85Jrdp4QKVw416AU=", "pLc9M0qPCrFKWQ6grY/FUw==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "4y1KDlXiBCzozHpPbKZmntM83HqZLlsHc3O5E0nZWdQ=", "pLc9M0qPCrFKWQ6grY/FUw==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "tJtA8dzvv7eViJPidpYnNkvU5OVoMHkRMnNbzeZnuRo=", "pLc9M0qPCrFKWQ6grY/FUw==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 4,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "Nmm06/bCuFcraGF8wMyJhd8LfzEfg/uE+5dOtzWPCvc=", "pLc9M0qPCrFKWQ6grY/FUw==" });

            migrationBuilder.CreateIndex(
                name: "IX_BookReservationEmailJobs_BookReservationId",
                table: "BookReservationEmailJobs",
                column: "BookReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_BookReservationJobs_BookReservationId",
                table: "BookReservationJobs",
                column: "BookReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_BookTransactionEmailJobs_BookTransactionId",
                table: "BookTransactionEmailJobs",
                column: "BookTransactionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookReservationEmailJobs");

            migrationBuilder.DropTable(
                name: "BookReservationJobs");

            migrationBuilder.DropTable(
                name: "BookTransactionEmailJobs");

            migrationBuilder.UpdateData(
                table: "BookReservations",
                keyColumn: "BookReservationId",
                keyValue: 1,
                column: "ReserveDate",
                value: new DateTime(2023, 6, 18, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "hVi2sPGUMgH7Log3LxDhiKC2VeMvX1E5+dWCHlCrLlQ=", "vmhYinW5OIRhBJy5Xmdp9w==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "4JzKGQl54kd+0FK/4Ub3289orQJP1Y0sMhNeyAWOMvE=", "vmhYinW5OIRhBJy5Xmdp9w==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "U65xncqgYLqAAetQ/De/W4AINRy8dzC4vhh2Cr2bK30=", "vmhYinW5OIRhBJy5Xmdp9w==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 4,
                columns: new[] { "PasswordHash", "PasswordSalt" },
                values: new object[] { "p3nf0HhjYTsRv4KhkV2kp0OK5C72SNj/Frny/844M7I=", "vmhYinW5OIRhBJy5Xmdp9w==" });
        }
    }
}
