using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Create_fnUserBorrowingHistory_Function : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE FUNCTION fnUserBorrowingHistory(@UserId INT)
                         RETURNS Table AS
						 RETURN
		                         SELECT 
									B.BookId,
									B.Title,
									dbo.fnBookAuthors(B.BookId) As Authors,
									BT.BorrowDate,
									BT.DueDate,
									BT.ReturnDate,
									BT.Fine
									
								FROM Books AS B
									INNER JOIN BookTransactions AS BT
								ON B.BookId = BT.BookId
								WHERE BT.UserId = @UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION fnUserBorrowingHistory");
        }
    }
}
