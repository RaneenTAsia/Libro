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
                name: "BookReservationJobs",
                columns: table => new
                {
                    BookReservationJobId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookReservationJobType = table.Column<int>(type: "int", nullable: false),
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
                name: "BookTransactionJobs",
                columns: table => new
                {
                    BookTransactionJobId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookTransactionJobType = table.Column<int>(type: "int", nullable: false),
                    BookTransactionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookTransactionJobs", x => x.BookTransactionJobId);
                    table.ForeignKey(
                        name: "FK_BookTransactionJobs_BookTransactions_BookTransactionId",
                        column: x => x.BookTransactionId,
                        principalTable: "BookTransactions",
                        principalColumn: "BookTransactionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookReservationJobs_BookReservationId",
                table: "BookReservationJobs",
                column: "BookReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_BookTransactionJobs_BookTransactionId",
                table: "BookTransactionJobs",
                column: "BookTransactionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookReservationJobs");

            migrationBuilder.DropTable(
                name: "BookTransactionJobs");
        }
    }
}
