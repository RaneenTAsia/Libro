using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Create_fnReadingListItems_Function : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE FUNCTION fnReadingListItems(@ReadingListId INT)
                                     RETURNS Table AS
						             RETURN
		                                     SELECT 
									            B.BookId,
									            B.Title,
									            dbo.fnBookAuthors(B.BookId) AS Authors,
									            dbo.fnBookGenres(B.BookId) AS Genres,
									            B.BookStatus	
								            FROM Books AS B
									            INNER JOIN ReadingItems AS RI
									            ON RI.BookId = B.BookId
									            WHERE RI.ReadingListId = @ReadingListId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION fnReadingListItems");
        }
    }
}
