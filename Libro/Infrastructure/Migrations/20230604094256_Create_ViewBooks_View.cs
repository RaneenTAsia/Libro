using Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Create_ViewBooks_View : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE VIEW ViewBooks AS
                SELECT
                    B.BookId,
                    B.Title,
                    dbo.fnBookAuthors(B.BookId) AS Authors,
                    dbo.fnBookGenres(B.BookId) AS Genres,
                    B.Description,
                    B.PageAmount,
                    B.PublishDate,
                    B.BookStatus
                FROM Books AS B");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP VIEW viewBooks");
        }
    }
}
