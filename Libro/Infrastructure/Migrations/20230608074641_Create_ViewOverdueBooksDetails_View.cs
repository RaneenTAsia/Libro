using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Create_ViewOverdueBooksDetails_View : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE VIEW ViewOverdueBooksDetails AS
                                    SELECT
		                                    B.BookId,
	                                        B.Title,
		                                    U.UserId,
		                                    U.Email
                                    FROM Books AS B
                                    INNER JOIN BookTransactions AS BT
                                    ON BT.BookId = b.BookId
                                    INNER JOIN Users AS U
                                    ON BT.UserId = U.UserId
                                    Where BT.ReturnDate IS NULL 
	                                    AND BT.DueDate < GETDATE()
                                    ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW ViewOverdueBooksDetails");
        }
    }
}
