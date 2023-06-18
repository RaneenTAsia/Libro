using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Create_fnBookReviews_Function : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE FUNCTION fnBookReviews(@BookId INT)
                         RETURNS Table AS
						 RETURN
		                         SELECT 
									u.Username,
                                    r.Rating,
                                    r.ReviewContent
									
								FROM Reviews AS R
                                INNER JOIN Users AS U
                                ON R.UserId = U.UserId
                                WHERE R.BookId = @BookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP FUNCTION fnBookReviews");
        }
    }
}
