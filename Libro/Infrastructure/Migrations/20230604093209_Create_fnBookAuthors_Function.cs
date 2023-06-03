using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Create_fnBookAuthors_Function : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"	CREATE FUNCTION fnBookAuthors(@BookId INT)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION fnBookAuthors");
        }
    }
}
