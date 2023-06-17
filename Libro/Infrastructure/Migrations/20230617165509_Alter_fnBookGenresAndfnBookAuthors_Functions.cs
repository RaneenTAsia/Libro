using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Alter_fnBookGenresAndfnBookAuthors_Functions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER FUNCTION fnBookGenres(@BookId INT)
                                     RETURNS VARCHAR(MAX) AS 
	                                     Begin
							             DECLARE @Result VARCHAR(MAX) = '';
		                                     SELECT @RESULT = STRING_AGG(BG.GENRE,',') 
		                                          From BooksToBookGenres As BTG
	                                         INNER JOIN BookGenres AS BG
   	                                              ON BTG.BookGenreId = BG.BookGenreId
			                                      WHERE BTG.BookId = @BookId
									              GROUP BY BTG.BookId
	                                     Return @Result
                                         END");

            migrationBuilder.Sql(@"	ALTER FUNCTION fnBookAuthors(@BookId INT)
									 RETURNS VARCHAR(MAX) AS 
										 BEGIN
											 DECLARE @Result VARCHAR(MAX) = '';

											 SELECT @Result = STRING_AGG(A.NAME,',')
												  From AuthorsToBooks As ATB
											 INNER JOIN Authors AS A
   												  ON ATB.AuthorId = A.AuthorId
												  WHERE ATB.BookId = @BookId
												  GROUP BY ATB.BookId
											 RETURN @RESULT
											END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER FUNCTION fnBookGenres(@BookId INT)
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

            migrationBuilder.Sql(@"	ALTER FUNCTION fnBookAuthors(@BookId INT)
									 RETURNS VARCHAR(MAX) AS 
										 BEGIN
											 DECLARE @Result VARCHAR(MAX) = ''

											 SELECT @Result = @Result + A.Name + ', '
												  From AuthorsToBooks As ATB
											 INNER JOIN Authors AS A
   												  ON ATB.AuthorId = A.AuthorId
												  WHERE ATB.BookId = @BookId
											 RETURN @RESULT
											END");
        }
    }
}
