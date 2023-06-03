using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Create_fnBookGenres_Function : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE FUNCTION fnBookGenres(@BookId INT)
                         RETURNS VARCHAR(MAX) AS 
	                         BEGIN
		                         DECLARE @Result VARCHAR(MAX) = ''

		                         SELECT @Result = @Result + BG.Genre + ', '
		                              From BooksToBookGenres As BTG
	                             INNER JOIN BookGenres AS BG
   	                                  ON BTG.BookGenreId = BG.BookGenreId
			                          WHERE BTG.BookId = @BookId
		                         RETURN @RESULT
	                            END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION fnBookGenres");
        }
    }
}
